using YIUIFramework;

namespace YIUICodeGenerated
{
    /// <summary>
    /// 由YIUI工具自动创建 请勿手动修改
    /// 用法: UIBindHelper.InternalGameGetUIBindVoFunc = YIUICodeGenerated.UIBindProvider.Get;
    /// </summary>
    public static class UIBindProvider
    {
        public static UIBindVo[] Get()
        {
            var BasePanel     = typeof(BasePanel);
            var BaseView      = typeof(BaseView);
            var BaseComponent = typeof(BaseComponent);
            var list          = new UIBindVo[10];
            list[0] = new UIBindVo
            {
                PkgName     = YIUI.Login.LoginPanelBase.PkgName,
                ResName     = YIUI.Login.LoginPanelBase.ResName,
                CodeType    = BasePanel,
                BaseType    = typeof(YIUI.Login.LoginPanelBase),
                CreatorType = typeof(YIUI.Login.LoginPanel),
            };
            list[1] = new UIBindVo
            {
                PkgName     = YIUI.Loading.LoadingPanelBase.PkgName,
                ResName     = YIUI.Loading.LoadingPanelBase.ResName,
                CodeType    = BasePanel,
                BaseType    = typeof(YIUI.Loading.LoadingPanelBase),
                CreatorType = typeof(YIUI.Loading.LoadingPanel),
            };
            list[2] = new UIBindVo
            {
                PkgName     = YIUI.Home.HomePanelBase.PkgName,
                ResName     = YIUI.Home.HomePanelBase.ResName,
                CodeType    = BasePanel,
                BaseType    = typeof(YIUI.Home.HomePanelBase),
                CreatorType = typeof(YIUI.Home.HomePanel),
            };
            list[3] = new UIBindVo
            {
                PkgName     = YIUI.Friends.FriendsPanelBase.PkgName,
                ResName     = YIUI.Friends.FriendsPanelBase.ResName,
                CodeType    = BasePanel,
                BaseType    = typeof(YIUI.Friends.FriendsPanelBase),
                CreatorType = typeof(YIUI.Friends.FriendsPanel),
            };
            list[4] = new UIBindVo
            {
                PkgName     = YIUI.Login.LanguagePopupViewBase.PkgName,
                ResName     = YIUI.Login.LanguagePopupViewBase.ResName,
                CodeType    = BaseView,
                BaseType    = typeof(YIUI.Login.LanguagePopupViewBase),
                CreatorType = typeof(YIUI.Login.LanguagePopupView),
            };
            list[5] = new UIBindVo
            {
                PkgName     = YIUI.Friends.FrindsListViewBase.PkgName,
                ResName     = YIUI.Friends.FrindsListViewBase.ResName,
                CodeType    = BaseView,
                BaseType    = typeof(YIUI.Friends.FrindsListViewBase),
                CreatorType = typeof(YIUI.Friends.FrindsListView),
            };
            list[6] = new UIBindVo
            {
                PkgName     = YIUI.Friends.FrindsMessagesViewBase.PkgName,
                ResName     = YIUI.Friends.FrindsMessagesViewBase.ResName,
                CodeType    = BaseView,
                BaseType    = typeof(YIUI.Friends.FrindsMessagesViewBase),
                CreatorType = typeof(YIUI.Friends.FrindsMessagesView),
            };
            list[7] = new UIBindVo
            {
                PkgName     = YIUI.Friends.FrindsRequestViewBase.PkgName,
                ResName     = YIUI.Friends.FrindsRequestViewBase.ResName,
                CodeType    = BaseView,
                BaseType    = typeof(YIUI.Friends.FrindsRequestViewBase),
                CreatorType = typeof(YIUI.Friends.FrindsRequestView),
            };
            list[8] = new UIBindVo
            {
                PkgName     = YIUI.Common.TopBarViewBase.PkgName,
                ResName     = YIUI.Common.TopBarViewBase.ResName,
                CodeType    = BaseView,
                BaseType    = typeof(YIUI.Common.TopBarViewBase),
                CreatorType = typeof(YIUI.Common.TopBarView),
            };
            list[9] = new UIBindVo
            {
                PkgName     = YIUI.Friends.FriendMessageItemBase.PkgName,
                ResName     = YIUI.Friends.FriendMessageItemBase.ResName,
                CodeType    = BaseComponent,
                BaseType    = typeof(YIUI.Friends.FriendMessageItemBase),
                CreatorType = typeof(YIUI.Friends.FriendMessageItem),
            };

            return list;
        }
    }
}