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
      render: (_: any, r: ProductApi.CategoryMo) => (
        <Space>
          {r.name}
          <TableButton
            func_code={FuncCodes.Product_SpuCategory_Manage}
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
      render: (_: any, r: ProductApi.CategoryMo) => (
        <Space>
          {r.order}

          <TableButton
            func_code={FuncCodes.Product_SpuCategory_Manage}
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
      render: (_: any, r: ProductApi.CategoryMo) => (
        <Space>
          <TableButton
            func_code={FuncCodes.Product_SpuCategory_Manage}
            onClick={() => {
              setParentCategory(r);
            }}
            icon={<AppstoreAddOutlined />}
          >
            添加子分类
          </TableButton>

          <TableFetchButton<ProductApi.CategoryMo>
            func_code={FuncCodes.Product_SpuCategory_Manage}
            record={r}
            callback={(res, item, aName) => {
              if (res.success) {
                categoryListReq.run();
              }
            }}
            btn_text="删除分类"
            fetch={(item: ProductApi.CategoryMo) => abandonCategory(item)}
            fetch_desp="删除当前分类"
            icon={<StopOutlined />}
          ></TableFetchButton>
        </Space>
      ),
    },
  ];

  const defaultCategory: any = { id: '-1' };

  const [parentCategory, setParentCategory] = useState<ProductApi.CategoryMo>(defaultCategory);
  const [editCategory, setEditCategory] = useState<ProductApi.CategoryMo>(defaultCategory);

  const [editType, setEditType] = useState(1);

  return (
    <PageContainer>
      {categoryListReq.data && (
        <ProTable<ProductApi.CategoryMo>
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
                func_code={FuncCodes.Product_SpuCategory_Manage}
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
            categoryListReq.run();
            setParentCategory(defaultCategory);
          }
        }}
        onClose={() => setParentCategory(defaultCategory)}
      ></AddCategory>

      <EditCategory
        record={editCategory}
        callback={(res) => {
          if (res.success) {
            categoryListReq.run();
            setEditCategory(defaultCategory);
          }
        }}
        edit_type={editType}
        onClose={() => setEditCategory(defaultCategory)}
      ></EditCategory>
    </PageContainer>
  );
}
