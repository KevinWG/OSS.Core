import React, { useRef } from 'react';
import { Modal, Button } from 'antd';
import { ModalProps } from 'antd/lib/modal';
import SearchForm from '@/components/search/search_form';
import SearchTable, { SearchTableAction } from '@/components/search/search_table';
import { FormItemFactoryProps } from '@/components/form/form_item_factory';
import { ColumnsType } from 'antd/lib/table';
import { BaseMo, SearchReq, PageListResp } from '@/utils/resp_d';

interface SelectTabelModalProps<T extends BaseMo> extends ModalProps {
  table_columns: ColumnsType<T>;
  search_form_items?: FormItemFactoryProps[];

  selected: (t: T) => void;
  search_fetch: (sReq: SearchReq) => Promise<PageListResp<T>>;
}

export default function SelectTabelModal<T extends BaseMo>(props: SelectTabelModalProps<T>) {
  const { selected, table_columns, search_fetch, search_form_items, ...restProps } = props;

  if (table_columns[table_columns.length - 1].key != 'SelectTabelModal_Choose') {
    table_columns.push({
      key: 'SelectTabelModal_Choose',
      title: '操作',
      dataIndex: 'id',
      render: (v: any, r: T) => (
        <Button
          type="primary"
          onClick={() => {
            selected(r);
          }}
        >
          选择
        </Button>
      ),
    });
  }

  const tableRef = useRef<SearchTableAction>();

  return (
    <Modal footer={null} {...restProps}>
      {search_form_items && search_form_items.length > 0 && (
        <SearchForm
          size="small"
          items={search_form_items}
          onFinish={(vals) => {
            tableRef.current?.reload(vals);
          }}
        ></SearchForm>
      )}
      <SearchTable<T>
        rowKey="id"
        size="small"
        columns={table_columns}
        search_table_ref={tableRef}
        search_fetch={search_fetch}
        pagination={{ pageSize: 10 }}
      />
    </Modal>
  );
}
