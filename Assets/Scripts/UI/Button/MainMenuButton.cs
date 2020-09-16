namespace TBSG.UI
{
    public class MainMenuButton : BaseButton
    {
        protected override void OnButtonClick()
        {
            base.OnButtonClick();
            GameManager.Instance.LoadMainMenu();
        }
    }
}