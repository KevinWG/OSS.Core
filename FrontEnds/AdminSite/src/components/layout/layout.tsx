import { getAllFuncItems } from '@/services/permit/func_api';
import { MenuDataItem, SettingDrawer } from '@ant-design/pro-components';
import { history, RunTimeLayoutConfig } from '@umijs/max';
import Footer from './Footer';
import { login_path } from './layout_const';
import RightContent from './RightContent';

function getFuncItem(code: string, funcItems: PermitApi.FuncItem[]) {
  for (let index = 0; index < funcItems.length; index++) {
    const element = funcItems[index];
    if (element.code == code) return element;
  }
  return undefined;
}

function loopItems(dm: MenuDataItem, allFuncItems: PermitApi.FuncItem[]): MenuDataItem {
  const children = dm.children?.map((c) => loopItems(c, allFuncItems));
  if (dm.access) {
    const funcItem = getFuncItem(dm.access, allFuncItems);
    if (funcItem) return { ...dm, name: funcItem.title, children: children };
  }
  return { ...dm, children: children };
}

async function loadRouteItems(_: any, defaultMenus: MenuDataItem[]) {
  const funcRes = await getAllFuncItems();
  const funcItems = funcRes.data ?? [];

  return defaultMenus.map((dm) => loopItems(dm, funcItems));
}

// ProLayout 支持的api https://procomponents.ant.design/components/layout
const layout: RunTimeLayoutConfig = ({ initialState, setInitialState }) => {
  return {
    rightContentRender: () => <RightContent />,
    disableContentMargin: false,
    waterMarkProps: {
      content: initialState?.currentUser?.name,
    },
    footerRender: () => <Footer />,
    onPageChange: () => {
      const { location } = history;
      // 如果没有登录，重定向到 login
      if (!initialState?.currentUser && location.pathname !== login_path) {
        history.push(login_path);
      }
    },
    menu: {
      locale: false,
      request: loadRouteItems,
    },

    menuHeaderRender: undefined,
    // 自定义 403 页面
    unAccessible: <div>unAccessible</div>,
    // 增加一个 loading 的状态
    childrenRender: (children, props) => {
      // if (initialState?.loading) return <PageLoading />;
      return (
        <>
          {children}
          {!props.location?.pathname?.includes('/login') && (
            <SettingDrawer
              disableUrlParams
              enableDarkTheme
              settings={initialState?.settings}
              onSettingChange={(settings) => {
                setInitialState((preInitialState) => ({
                  ...preInitialState,
                  settings,
                }));
              }}
            />
          )}
        </>
      );
    },
    ...initialState?.settings,
  };
};

export default layout;
