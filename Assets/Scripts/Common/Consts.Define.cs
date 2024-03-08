using UnityEngine;

namespace Consts
{
    public class Def
    {
        public static readonly float DropEntityHeight = 0.59f;
        public static readonly float DiffHeight = 5f;
        public static readonly float SpeedOrigin = 3f;
        public static readonly float RibbonGap = 7f;
        public static readonly int FirstTryCount = 5;
        public static readonly int InitTryCount = 3;

        public static readonly Vector3[] DirVecs =
        {
            new Vector3(0, 1, 0),
            new Vector3(0, -1, 0),
            new Vector3(-1, 0, 0),
            new Vector3(1, 0, 0),
        };

        public static readonly string[] URLs =
        {
            "https://play.google.com/store/search?q=cj%EB%8D%94%EB%A7%88%EC%BC%93&c=apps&pli=1",
            "https://www.instagram.com/ndolphinconnect/",
            "https://www.facebook.com/%EC%97%94%EB%8F%8C%ED%95%80%EC%BB%A4%EB%84%A5%ED%8A%B8-103441671801072",
            "https://www.youtube.com/channel/UCkHRdqi1NBY5i64jXPOjYGg/featured"
        };
    }
}