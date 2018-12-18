using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Treal;
using UnityEngine;

public class DrawContent : MonoBehaviour, IContent
{
    public const string DRAW_PLAYDATA = "DrawPlayData.tro";
    public const string DUC_TEXTURE = "drawdone.png";

    [SerializeField]
    public DrawPlayer mDrawPlayer;
    [SerializeField]
    public SpriteRenderer mSprite;

    #region IContent
    public KidsError LoadContent(KContent content)
    {
        loadTimeline(content);
        loadDUC(content);

        return KidsError.None;
    }


    public void Pause()
    {
        mDrawPlayer.Pause();
    }

    public void Play()
    {
        mDrawPlayer.StartPlay();
    }

    public void SetTime(float time)
    {
        mDrawPlayer.SetTime(time);
    }

    public void Stop()
    {
        mDrawPlayer.Stop();
    }

    public void UnloadContent()
    {
        mDrawPlayer.ClearPlayData();
        DestroyImmediate(gameObject, true);
    }

    public void Show()
    {
        mSprite.enabled = true;
    }

    public void Hide()
    {
        mSprite.enabled = false;
    }
    #endregion

    #region private
    private void loadTimeline(KContent content)
    {
        var data = DrawPlayLoader.LoadDrawPlayData(
                        Path.Combine(content.path, DRAW_PLAYDATA));
        mDrawPlayer.LoadPlayData(data);
    }

    private bool loadDUC(KContent content)
    {
        var bytes = File.ReadAllBytes(Path.Combine(content.path, DUC_TEXTURE));
        var texture = new Texture2D(2, 2);
        if (!texture.LoadImage(bytes))
        {
            //Load Image Fail
            return false;
        }
        
        mSprite.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5f, .5f));
        return true;
    }


    #endregion
}
