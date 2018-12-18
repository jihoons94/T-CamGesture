using System;

[Serializable]
public class KContentInfo {
    public string cid;
    public KContent[] contents;
}

[Serializable]
public class KContent {
    public string type;
    public string path;
    public string sticker;
}