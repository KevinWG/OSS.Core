import AccessButton from '@/components/button/access_button';
import TableButton from '@/components/button/table_button';
import FuncCodes from '@/services/common/func_codes';
import { getAllFuncTreeItems } from '@/services/permit/func_api';
import { PageContainer, ProColumns, ProTable } from '@ant-design/pro-components';
import { useRequest } from 'ahooks';
import { Space } from 'antd';
import React, { useState } from 'react';
import FuncItemEditor from './FuncItemEditor';

const FuncItemList: React.FC = () => {
  const defaultItem: any = { id: '' };

  const [editItem, setEditItem] = useState<PermitApi.FuncItem>(defaultItem);
  // const actionRef = useRef<ActionType>();

  const tableCols: ProColumns<PermitApi.FuncItem>[] = [
    {
      title: '名称',
      dataIndex: 'title',
    },
    {
      title: '编码',
      dataIndex: 'code',
    },
    {
      title: '操作',
      dataIndex: 'code',
      render: (dom: React.ReactNode, r: PermitApi.FuncItem) => {
        return (
          <Space>
            <TableButton
              func_code={FuncCodes.Portal_Func_Operate}
              onClick={() => {
                setEditItem(r);
              }}
            >
              编辑
            </TableButton>
            <TableButton
              func_code={FuncCodes.Portal_Func_Operate}
              onClick={() => {
                setEditItem({ ...r, id: '0' });
              }}
            >
              增加子权限
            </TableButton>
          </Space>
        );
      },
    },
  ];
  const treeReq = useRequest(getAllFuncTreeItems);
  return (
    <PageContainer title={false}>
      {treeReq.data && (
        <ProTable<PermitApi.FuncItem>
          rowKey="code"
          search={false}
          pagination={false}
          columns={tableCols}
          defaultExpandAllRows={true}
          indentSize={20}
          dataSource={treeReq.data?.data}
          loading={treeReq.loading}
          headerTitle="权限项管理"
          toolbar={{
            actions: [
              <AccessButton
                key="primary"
                type="primary"
                func_code={FuncCodes.Portal_Func_Operate}
                onClick={() => {
                  setEditItem({ id: '0' } as any);
                }}
              >
                新增权限项
              </AccessButton>,
            ],
          }}
        ></ProTable>
      )}
      <FuncItemEditor
        record={editItem}
        visible={!!editItem.id}
        onCancel={() => setEditItem(defaultItem)}
        callback={(res: IResp, item?: PermitApi.FuncItem) => {
          if (res.success) {
            setEditItem(defaultItem);
            treeReq.run();
            // actionRef.current?.reload();
          }
        }}
      ></FuncItemEditor>
    </PageContainer>
  );
};

export default FuncItemList;
