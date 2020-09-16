namespace TBSG.UI
{
    public class PlayButton : BaseButton
    {
        protected override void OnButtonClick()
        {
            base.OnButtonClick();
            GameManager.Instance.LoadGame();
        }
    }
}