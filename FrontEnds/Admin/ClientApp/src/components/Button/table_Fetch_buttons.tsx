import { useRequest } from 'umi';
import { message, Space } from 'antd';
import React from 'react';
import FetchConfirmButton from '@/components/button/fetch_confirm_button';
import { BaseMo, Resp } from '@/utils/resp_d';

interface TableFetchButtonProps<T> {
  record: T;
  /**
   * 如果涉及服务端接口交互，实现此方法即可
   */
  fetch: (record: T) => Promise<Resp>;
  /**
   * 按钮名称，回调函数会传回
   */
  btn_name?: string;
  func_code?: string;
  icon?: React.ReactNode;
  /**
   * 提示消息中使用的动作描述
   */
  fetch_desp?: string;
  /**
   * 按钮显示文字
   */
  btn_text?: string;

  /**
   * 如果仅仅是页面处理，在此事件执行即可
   */
  btn_click?: (record: T) => void;

  fetchKey?: (record: T) => string;
  callback?: (res: Resp, item: T, actionName?: string) => void;
}


// 锁定按钮
export function TableFetchButton<T>(props: TableFetchButtonProps<T>) {
  const { btn_text, record, icon, func_code, callback, fetch, fetchKey } = props;

  let aBtnProps: any = {
    type: 'dashed',
    shape: 'round',
    size: 'small',
    icon: icon,
    func_code: func_code,
    onClick: () => {
      if (props.btn_click) {
        props.btn_click(record);
      }
    },
  };

  if (fetch) {
    const dispalyName = props.fetch_desp ?? btn_text ?? '执行';
    const doAction = async (item: T) => {
      message.info('开始 ' + btn_text);
      var res = await fetch(item);
      if (res.is_ok) {
        message.success(btn_text + ' 成功！');
        if (callback) callback(res, item, btn_text);
      } else {
        message.error(btn_text + ' 失败:' + res.msg);
      }
    };

    const reqHandler = useRequest(doAction, { manual: true, fetchKey: fetchKey });

    aBtnProps.loading = fetchKey
      ? reqHandler.fetches[fetchKey(record)]?.loading
      : reqHandler.loading;

    aBtnProps.confirm_props = {
      title: '是否确认' + dispalyName,
      onConfirm: () => reqHandler.run(record),
      okText: '确认',
      cancelText: '放弃',
    };
  }

  return <FetchConfirmButton {...aBtnProps}>{btn_text}</FetchConfirmButton>;
}

interface TableFetchButtonsProps<T extends BaseMo> {
  record: T;
  children?: React.ReactNode;
  condition_buttons?: {
    condition: (r: T) => boolean;
    buttons: Omit<TableFetchButtonProps<T>, 'callback' | 'record' | 'fetchKey'>[];
  }[];
  fetchKey?: (record: T) => string;
  callback?: (res: Resp, item: T, actionName?: string) => void;
}
export default function TableFetchButtons<T extends BaseMo>(
  props: TableFetchButtonsProps<T>,
): JSX.Element {
  const { record, condition_buttons, callback, fetchKey } = props;
  // const s = record?.status;

  return (
    <Space>
      {condition_buttons &&
        condition_buttons.map(
          (cBtns) =>
            cBtns.condition(record) &&
            cBtns.buttons.map((btn) => (
              <TableFetchButton<T>
                key={'table_btn_' + record?.id}
                record={record}
                callback={callback}
                fetchKey={fetchKey}
                {...btn}
              />
            )),
        )}
      {props.children}
    </Space>
  );
}
