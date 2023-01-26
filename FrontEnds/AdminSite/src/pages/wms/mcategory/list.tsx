import AccessButton from '@/components/button/access_button';
import TableButton from '@/components/button/table_button';
import { TableFetchButton } from '@/components/button/table_Fetch_buttons';
import FuncCodes from '@/services/common/func_codes';
import { AppstoreAddOutlined, EditOutlined, StopOutlined } from '@ant-design/icons';
import { PageContainer, ProTable } from '@ant-design/pro-components';
import { useRequest } from 'ahooks';
import { Space } from 'antd';
import { useState } from 'react';
import AddCategory from './compents/add_category';
import EditCategory from './compents/edit_category';
import { abandonCategory, category_search } from './service';

export default function () {
  const categoryListReq = useRequest(category_search);

  const tableColumns = [
    {
      title: '名称',
      dataIndex: 'name',
      key: 'name',
      render: (_: any, r: WmsApi.CategoryMo) => (
        <Space>
          {r.name}
          <TableButton
            func_code={FuncCodes.WMS_MCategory_Manage}
            onClick={() => {
              setEditType(1);
              setEditCategory(r);
            }}
            icon={<EditOutlined />}
          ></TableButton>
        </Space>
      ),
    },
    {
      title: '排序编号',
      dataIndex: 'order',
      key: 'order',
      render: (_: any, r: WmsApi.CategoryMo) => (
        <Space>
          {r.order}

          <TableButton
            func_code={FuncCodes.WMS_MCategory_Manage}
            onClick={() => {
              setEditType(2);
              setEditCategory(r);
            }}
            icon={<EditOutlined />}
          ></TableButton>
        </Space>
      ),
    },
    {
      title: '操作',
      dataIndex: 'id',
      render: (_: any, r: WmsApi.CategoryMo) => (
        <Space>
          <TableButton
            func_code={FuncCodes.WMS_MCategory_Manage}
            onClick={() => {
              setParentCategory(r);
            }}
            icon={<AppstoreAddOutlined />}
          >
            添加子分类
          </TableButton>

          <TableFetchButton<WmsApi.CategoryMo>
            func_code={FuncCodes.WMS_MCategory_Manage}
            record={r}
            callback={(res, item, aName) => {
              if (res.success) {
                categoryListReq.run();
              }
            }}
            btn_text="作废分类"
            fetch={(item: WmsApi.CategoryMo) => abandonCategory(item)}
            fetch_desp="作废当前分类"
            icon={<StopOutlined />}
          ></TableFetchButton>
        </Space>
      ),
    },
  ];

  const defaultCategory: any = { id: '-1' };

  const [parentCategory, setParentCategory] = useState<WmsApi.CategoryMo>(defaultCategory);
  const [editCategory, setEditCategory] = useState<WmsApi.CategoryMo>(defaultCategory);

  const [editType, setEditType] = useState(1);

  return (
    <PageContainer>
      {categoryListReq.data && (
        <ProTable<WmsApi.CategoryMo>
          search={false}
          pagination={false}
          indentSize={20}
          rowKey="id"
          columns={tableColumns}
          dataSource={categoryListReq.data.data}
          defaultExpandAllRows={true}
          toolbar={{
            actions: [
              <AccessButton
                type="primary"
                func_code={FuncCodes.WMS_MCategory_Manage}
                // func_code={FuncCodes.portal_role_add}
                onClick={() => {
                  setParentCategory({ id: '0' } as any);
                }}
              >
                添加一级分类
              </AccessButton>,
            ],
          }}
        ></ProTable>
      )}

      <AddCategory
        record={parentCategory}
        callback={(res) => {
          if (res.success) {
            setParentCategory(defaultCategory);
            categoryListReq.run();
          }
        }}
        onClose={() => setParentCategory(defaultCategory)}
      ></AddCategory>

      <EditCategory
        record={editCategory}
        callback={(res) => {
          if (res.success) {
            setEditCategory(defaultCategory);
            categoryListReq.run();
          }
        }}
        edit_type={editType}
        onClose={() => setEditCategory(defaultCategory)}
      ></EditCategory>
    </PageContainer>
  );
}
