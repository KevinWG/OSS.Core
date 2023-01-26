import AccessButton from '@/components/button/access_button';
import TableFetchButtons from '@/components/button/table_Fetch_buttons';
import SearchProTable from '@/components/search/search_protable';
import { CommonStatus } from '@/services/common/enums';
import FuncCodes from '@/services/common/func_codes';
import { EditOutlined, LockOutlined, UnlockOutlined } from '@ant-design/icons';
import {
  ActionType,
  PageContainer,
  ProColumns,
  ProFormTreeSelect,
} from '@ant-design/pro-components';
import moment from 'moment';
import { useRef, useState } from 'react';
import { category_search } from '../mcategory/service';
import MaterialEditor from './components/material_editor';
import { MaterialType, searchMaterials, setUabale } from './service';

function ShowMultiText(r: WmsApi.Material) {
  if (r.multi_units && r.multi_units.length > 0) {
    // let text = '';
    // r.multi_units.forEach((ele) => {
    //   text += '1 ' + r.basic_unit + '=' + ele.ratio + ' ' + ele.unit + '<br/>';
    // });
    // return text;
    return r.multi_units.map((ele) => (
      <div>{'1 ' + ele.unit + '=' + ele.ratio + ' ' + r.basic_unit}</div>
    ));
  }
  return '无';
}

export default function MetarialList() {
  const statusButtons = [
    {
      condition: (r: WmsApi.Material) => r.status == 0,
      buttons: [
        {
          title: '禁用',
          fetch: (item: WmsApi.Material) => setUabale(item.id, 0),
          fetch_desp: '禁用当前物料',
          func_code: FuncCodes.wms_material_manage,
          icon: <LockOutlined />,
        },
        {
          title: '编辑',
          icon: <EditOutlined />,
          func_code: FuncCodes.wms_material_manage,
          btn_click: (r: WmsApi.Material) => {
            setEditItem(r);
          },
        },
      ],
    },
    {
      condition: (r: WmsApi.Material) => r.status == -100,
      buttons: [
        {
          title: '解禁',
          fetch: (item: WmsApi.Material) => setUabale(item.id, 1),
          fetch_desp: '解禁当前物料',
          func_code: FuncCodes.wms_material_manage,
          icon: <UnlockOutlined />,
        },
      ],
    },
  ];

  const tableColumns: ProColumns<WmsApi.Material>[] = [
    {
      title: '编码',
      dataIndex: 'code',
    },
    {
      title: '名称',
      dataIndex: 'name',
    },
    {
      title: '分类',
      dataIndex: 'c_id',
      hideInTable: true,
      valueType: 'treeSelect',
      renderFormItem: (_, { type, defaultRender, ...rest }, form) => {
        if (type === 'form') {
          return null;
        }
        return (
          <ProFormTreeSelect
            name="c_id"
            width="md"
            {...rest}
            request={async () =>
              category_search().then((res) => {
                if (res.success) return res.data;
                return [];
              })
            }
            fieldProps={{
              allowClear: true,
              fieldNames: {
                label: 'name',
                value: 'id',
              },
              treeDefaultExpandAll: true,
              placeholder: '请选择物料分组',
            }}
          />
        );
      },
    },
    {
      title: '类型',
      dataIndex: 'type',
      valueType: 'select',
      valueEnum: MaterialType,
    },
    {
      title: '最小单位',
      hideInSearch: true,
      dataIndex: 'basic_unit',
    },
    {
      title: '多单位',
      hideInSearch: true,
      dataIndex: 'multi_units',
      render: (_, e) => ShowMultiText(e),
    },

    {
      title: '规格参数',
      hideInSearch: true,
      dataIndex: 'tec_spec',
    },
    {
      title: '原厂型号',
      hideInSearch: true,
      dataIndex: 'factory_serial',
    },
    {
      title: '备注',
      hideInSearch: true,
      dataIndex: 'remark',
    },
    {
      title: '状态',
      dataIndex: 'status',
      valueType: 'select',
      valueEnum: CommonStatus,
    },
    {
      title: '创建时间',
      dataIndex: 'add_time',
      hideInSearch: true,
      render: (_, e) => moment(e.add_time * 1000).format('YYYY-MM-DD'),
    },
    {
      title: '操作',
      dataIndex: 'id',
      hideInSearch: true,
      render: (_: any, r: WmsApi.Material) => (
        <TableFetchButtons
          record={r}
          callback={(res, item, aName) => {
            if (res.success) tableRef.current?.reload();
          }}
          fetchKey={(item) => item.id}
          condition_buttons={statusButtons}
        ></TableFetchButtons>
      ),
    },
  ];

  const tableRef = useRef<ActionType>();
  const defaultItem = { id: '-1' } as WmsApi.Material;
  const [editItem, setEditItem] = useState(defaultItem);

  return (
    <PageContainer>
      <SearchProTable<WmsApi.Material>
        rowKey="id"
        size="small"
        columns={tableColumns}
        actionRef={tableRef}
        request={searchMaterials}
        toolbar={{
          actions: [
            <AccessButton
              type="primary"
              func_code={FuncCodes.wms_material_manage}
              onClick={() => {
                setEditItem({ id: '0' } as any);
              }}
            >
              添加物料
            </AccessButton>,
          ],
        }}
      />
      <MaterialEditor
        open={editItem.id != '-1'}
        drawerProps={{
          onClose: () => setEditItem(defaultItem),
        }}
        callback={(res) => {
          if (res.success) {
            setEditItem(defaultItem);
            tableRef.current?.reload();
          }
        }}
        record={editItem}
      ></MaterialEditor>
    </PageContainer>
  );
}
