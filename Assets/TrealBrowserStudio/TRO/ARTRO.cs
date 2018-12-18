using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Treal.BrowserCore;
using Treal.Browser.Core;
using Treal.Browser.Log;

namespace Treal.Browser.AR
{
    public class ARTRO : ITRO
    {
        private ITRA mParent;

        private string mId;
        private bool mIsMain;
        private string mName;
        private string mVersion;

        private string mPath;

        public string ID { get { return mId; } }
        public bool IsMain { get { return mIsMain; } }
        public string Name { get { return mName; } }

        public string Version { get { return mVersion; } }

        private bool isLoaded = false;


        public ARTRO() { }
        public ARTRO(ITRA tra, TROJsonInfo info, string path = null)
        {
            mParent = tra;

            mId = info.troID;
            mName = info.troFileName;
            mVersion = info.troVer;

            mPath = path;
        }

        public void Load()
        {
            Treal_Logger.SendLog(ActionCode.MGR_START, mParent.ProductId);

            if (isLoaded)
            {
                TCamManager.Instance.StartPreview();
                TCamManager.Instance.StartImageTracking();
                return;
            }

            if (TCamManager.Instance == null)
            {
                UnityEngine.Debug.Log("T Cam Manager Missing!");
                return;
            }


            // get dcs path
            string descPath = string.Empty;

            if (string.IsNullOrEmpty(mPath))
            {
                descPath = Path.Combine(mParent.GetPath, mName);
            }
            else
            {
                descPath = mPath;
            }
            

            // load mapping file
            MappingDataList mappingDataList = null;
            var mapTro = mParent.GetTRO(mName.Split('.')[0] + ".map");
            mapTro.Load();
            mappingDataList = ((MetaTRO)mapTro).mappingDataList;

            // go through mapping data list
            for (int i = 0; i < mappingDataList.mappinglist.Count; i++)
            {
                // create target image
                GameObject targetImage = new GameObject(mappingDataList.mappinglist[i].desc_name, typeof(CTargetImage));
                CTargetImage ti = targetImage.GetComponent<CTargetImage>();

                ti.ARTro = this;

                // set uuid & name
                ti.TargetName = mappingDataList.mappinglist[i].desc_name;
                ti.TargeUUID = mappingDataList.mappinglist[i].desc_id;

                targetImage.transform.SetParent(CTrackingManager.Instance.transform);

                List<ITRO> contentsTROList = new List<ITRO>();

                // load contents tro
                for (int j = 0; j < mappingDataList.mappinglist[i].tro_list.Count; j++)
                {
                    string contentsName = mappingDataList.mappinglist[i].tro_list[j].contents_tro_name += ".tro";

                    Debug.Log("contentsName: " + contentsName);

                    var contentsTro = mParent.GetTRO<ContentTRO>(contentsName);

                    // set contents TRO transform
                    contentsTro.position = mappingDataList.mappinglist[i].tro_list[j].position;
                    contentsTro.rotation = mappingDataList.mappinglist[i].tro_list[j].rotation;
                    contentsTro.scale = mappingDataList.mappinglist[i].tro_list[j].scale;

                    contentsTROList.Add(contentsTro);
                }

                ti.contentsTROList = contentsTROList;

                CTrackingManager.Instance._targetImageList.Add(ti);
            }

            // set tracking handler
            TrealBrowser.OnARStart += OnARStart;
            TrealBrowser.OnARFound += OnARFound;
            TrealBrowser.OnARLost += OnARLost;

            CTrackingManager.Instance.LoadDescriptor(descPath);

            TCamManager.Instance.StartPreview();
            TCamManager.Instance.StartImageTracking();

            isLoaded = true;
        }

        public IEnumerator CoLoad()
        {
            Load();
            yield return null;
        }

        public void Start()
        {
            Load();
        }

        public IEnumerator CoStart()
        {
            yield return CoLoad();
        }

        public void Stop()
        {
            Unload();
        }

        public void Unload()
        {
            //CTrackingManager.Instance.UnloadDescriptor();

            TCamManager.Instance.StopImageTracking();
            TCamManager.Instance.StopPreview();

            Treal_Logger.SendLog(ActionCode.MGR_END, mParent.ProductId);
        }

        public void Update()
        {

        }

        public ITRA GetTRA()
        {
            return mParent;
        }

        public int GetTypeNumber()
        {
            return (int)TROType.Descriptor_AR;
        }

        public string GetExtension()
        {
            return ".dcs";
        }

        private void OnARStart(ITRO tro, object targetImage, List<ITRO> contentsTROList)
        {
            var ti = targetImage as CTargetImage;

            foreach (var item in contentsTROList)
            {
                var content = item as ContentTRO;
                content.Load();

                var go = Object.Instantiate(content.GetGameObject(), ti.transform);
                go.transform.localPosition = content.position;
                go.transform.localEulerAngles = content.rotation;
                go.transform.localScale = content.scale;
                go.SetActive(false);
            }
        }

        private void OnARFound(ITRO tro, object targetImage, List<ITRO> contentsTROList)
        {
            var ti = (CTargetImage)targetImage;

            var childCount = ti.transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                ti.transform.GetChild(i).gameObject.SetActive(true);
            }
            
            Debug.Log("Found: " + ti.name);
        }

        private void OnARLost(ITRO tro, object targetImage, List<ITRO> contentsTROList)
        {
            var ti = (CTargetImage)targetImage;

            var childCount = ti.transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                ti.transform.GetChild(i).gameObject.SetActive(false);
            }

            Debug.Log("Lost: " + ti.name);
        }
    }
}