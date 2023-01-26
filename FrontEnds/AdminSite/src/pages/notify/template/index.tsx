import AccessButton from '@/components/button/access_button';
import TableButton from '@/components/button/table_button';
import TableFetchButtons from '@/components/button/table_Fetch_buttons';
import SearchProTable, { getTextFromLabelValues } from '@/components/search/search_protable';
import { CommonStatus } from '@/services/common/enums';
import FuncCodes from '@/services/common/func_codes';
import {
  NotifyChannel,
  NotifyType,
  searchTemplate,
  setUseable,
} from '@/services/notify/template_api';
import { ActionType, PageContainer, ProColumns } from '@ant-design/pro-components';
import { Space } from 'antd';
import moment from 'moment';
import { useRef, useState } from 'react';
import TemplateEditor from './template_editor';

const cols: ProColumns<NotifyApi.Template>[] = [
  { title: '模板Id', dataIndex: 'id', hideInSearch: true, copyable: true },
  { title: '标题', dataIndex: 'title' },
  {
    title: '发送类型',
    dataIndex: 'notify_type',
    valueType: 'select',
    fieldProps: { options: NotifyType },
    renderText: (_, e) => getTextFromLabelValues(e.notify_type, NotifyType),
  },
  {
    title: '推送通道',
    dataIndex: 'notify_channel',
    valueType: 'select',
    fieldProps: { options: NotifyChannel },
    renderText: (_, e) => getTextFromLabelValues(e.notify_channel, NotifyChannel),
  },
  {
    title: '创建时间',
    dataIndex: 'add_time',
    hideInSearch: true,
    renderText: (_, e) => moment(e.add_time * 1000).format('YYYY-MM-DD'),
  },
  {
    title: '状态',
    dataIndex: 'status',
    initialValue: '0',
    valueType: 'select',
    valueEnum: CommonStatus,
  },
];

const defaultEditItem: any = { id: '' };

const TemplateList: React.FC = () => {
  const [editItem, setEditItem] = useState<NotifyApi.Template>(defaultEditItem);
  const tableCols: ProColumns<NotifyApi.Template>[] = [
    ...cols,
    {
      title: '操作',
      hideInSearch: true,
      render(dom, entity, index, action, schema) {
        return (
          <Space>
            <TableButton
              func_code={FuncCodes.Notify_Template_Update}
              onClick={() => {
                setEditItem(entity);
              }}
            >
              修改
            </TableButton>

            <TableFetchButtons
              record={entity}
              callback={(res, r) => {
                if (res.success) {
                  tableAction.current?.reload();
                }
              }}
              condition_buttons={[
                {
                  condition: (r) => r.status == 0,
                  buttons: [{ fetch: (r) => setUseable(r.id, 0), btn_text: '作废' }],
                },
                {
                  condition: (r) => r.status == -100,
                  buttons: [{ fetch: (r) => setUseable(r.id, 1), btn_text: '启用' }],
                },
              ]}
            ></TableFetchButtons>
          </Space>
        );
      },
    },
  ];
  const tableAction = useRef<ActionType>();
  return (
    <PageContainer>
      <SearchProTable
        columns={tableCols}
        rowKey="id"
        search={{ defaultCollapsed: false }}
        // form={{ initialValues: { status: '0' } }}
        request={searchTemplate}
        actionRef={tableAction}
        toolbar={{
          actions: [
            <AccessButton
              type="primary"
              func_code={FuncCodes.Notify_Template_Add}
              onClick={() => {
                setEditItem({ id: '0' } as any);
              }}
            >
              新增模板
            </AccessButton>,
          ],
        }}
      ></SearchProTable>
      <TemplateEditor
        visible={!!editItem.id}
        onCancel={() => setEditItem(defaultEditItem)}
        record={editItem}
        callback={(res, r) => {
          if (res.success) {
            setEditItem(defaultEditItem);
            tableAction.current?.reload();
          }
        }}
      ></TemplateEditor>
    </PageContainer>
  );
};
export default TemplateList;
