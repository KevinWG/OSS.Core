import React, { useState } from 'react';
import { FormItemFactoryProps } from '@/components/form/form_item_factory';
import SelectTabelModal from '@/components/modal/search_table_modal';
import { Button, Space } from 'antd';
import { ColumnsType } from 'antd/lib/table';
import { SearchReq, PageListResp, BaseMo } from '@/utils/resp_d';
import { ModalProps } from 'antd/lib/modal';

interface FormItemSelectElementProps<T extends BaseMo>
  extends Omit<ModalProps, 'visible' | 'onCancel'> {
  /**
   * 自定义组件需要定义，由Form.Item 自动包装传值
   */
  value?: T | string;
  /**
   * 自定义组件需要定义，由Form.Item 自动包装传值
   */
  onChange?: (v: T) => void;

  display: (t?: T) => React.ReactNode | string;
  search_fetch: (sReq: SearchReq) => Promise<PageListResp<T>>;

  table_columns: ColumnsType<T>;
  search_form_items?: FormItemFactoryProps[];
}

export default function FormItemSelectElement<T extends BaseMo>(
  props: FormItemSelectElementProps<T>,
) {
  const {
    value,
    onChange,
    display,
    table_columns,
    search_form_items,
    search_fetch,
    ...restProps
  } = props;

  const [showModal, setShowModal] = useState(false);
  const [chooseInfo, setChooseInfo] = useState<T>(value as T);

  return (
    <>
      <Space>
        <span>{display(chooseInfo) || value}</span>
        <Button
          type="primary"
          onClick={() => {
            setShowModal(true);
          }}
        >
          选择
        </Button>
      </Space>

      <SelectTabelModal<T>
        {...restProps}
        visible={showModal}
        onCancel={() => {
          setShowModal(false);
        }}
        style={{ minWidth: 580 }}
        search_form_items={search_form_items}
        search_fetch={search_fetch}
        table_columns={table_columns}
        selected={(r) => {
          setChooseInfo(r);
          setShowModal(false);
          if (onChange) {
            onChange(r);
          }
        }}
      ></SelectTabelModal>
    </>
  );
}
