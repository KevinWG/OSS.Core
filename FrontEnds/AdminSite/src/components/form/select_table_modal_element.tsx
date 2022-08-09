import SelectTabelModal, { SelectTabelModalProps } from '@/components/modal/search_table_modal';
import { ParamsType } from '@ant-design/pro-components';
import { Button, ModalProps, Space } from 'antd';
import React, { useEffect, useState } from 'react';

interface SelectTableModalElementProps<T extends BaseMo, U, ValueType>
  extends Omit<SelectTabelModalProps<T, U, ValueType>, 'onChange' | 'selected' | 'modal_props'> {
  /**
   * 自定义组件需要定义，由Form.Item 自动包装传值
   */
  value?: any;

  /**
   * 自定义组件需要定义，由Form.Item 自动包装传值
   */
  onChange?: (v: any) => void;

  /**
   *  选中之后值的转换
   */
  value_change_transform?: (v: T) => any;

  value_initial?: (v: any) => Promise<T>;

  display: (t?: T) => React.ReactNode | string | undefined;

  modal_props?: ModalProps;
}

export default function SelectTableModalElement<
  T extends BaseMo,
  Params extends ParamsType = ParamsType,
  ValueType = 'text',
>(props: SelectTableModalElementProps<T, Params, ValueType>) {
  const {
    value,
    onChange,
    display,
    modal_props,
    value_change_transform,
    value_initial,
    ...restProps
  } = props;

  const [showModal, setShowModal] = useState(false);
  const [chooseInfo, setChooseInfo] = useState<T>();

  useEffect(() => {
    if (!chooseInfo) {
      if (value_initial) value_initial(value).then((res) => setChooseInfo(res));
      else setChooseInfo(value as any);
    }
  }, []);

  return (
    <>
      <Space
        onClick={() => {
          setShowModal(true);
        }}
      >
        {display(chooseInfo) || <Button type="primary">选择</Button>}
      </Space>

      <SelectTabelModal<T, Params, ValueType>
        modal_props={{
          style: { minWidth: 580, width: 800 },
          ...modal_props,
          visible: showModal,
          onCancel: () => {
            setShowModal(false);
          },
        }}
        toolBarRender={false}
        selected={(r) => {
          setChooseInfo(r);
          setShowModal(false);
          if (onChange) {
            onChange(value_change_transform ? value_change_transform(r) : r);
          }
        }}
        {...restProps}
      ></SelectTabelModal>
    </>
  );
}
