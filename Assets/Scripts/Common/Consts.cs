namespace Consts
{
    public enum eMsg
    {
        // 파이어베이스 로그인 성공 시 호출
        OnLoginSuccess,
        
        // 코인 UI 업데이트
        OnUpdateCoinUI,
        
        // 카메라 수직 상승
        OnUpdateCamera,
        
        // 콜리전 수직 상승
        OnUpdateCollision,
    }
    
    public enum eCamMode
    {
        Normal,
        Wait,
        Follow
    }
    
    public enum eEntity
    {
        Normal = 0,
        Bonus = 1,
        Obstacle = 2
    }

    public enum eEffect
    {
        Bonus = 0,
        Obstacle,
        Round,
    }
    
    public enum eDir
    {
        Left,
        Right,
        Down,
        Up
    }
    
    public enum eBgm
    {
        Opening,
        Main,
        Gameover
    }
    
    public enum eSfx
    {
        Button,
        Landing,
        Clear,
    }

    public enum eURL
    {
        Google,
        Instagram,
        Facebook,
        Youtube
    }

    public enum eLauncherMsg
    {
        /// <summary>
        /// 게임 시작 시 초기 설정
        /// </summary>
        Init,
        
        /// <summary>
        /// 유니티 인스턴스 완료 시점에 전송
        /// </summary>
        Load,
        
        /// <summary>
        /// 게임 로딩 완료 시점(게임 UI 로딩 완료)에 런처로 전송
        /// </summary>
        Loaded,
        
        /// <summary>
        /// 탑쌓기 게임 완료 시 스코어 전송
        /// </summary>
        Submit
    }
}