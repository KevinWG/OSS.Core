import { DrawerProps } from 'antd/lib/drawer';

import EditDrawerForm from '@/components/form/eidt_drawer_form';
import { FormItemFactoryProps, FormItemFactoryType } from '@/components/form/form_item_factory';
import { createCategory } from '../service';

interface AddCategoryProps extends IEditorProps<ProductApi.CategoryMo>, DrawerProps {}

export default function AddCategory(props: AddCategoryProps) {
  const { callback, record, ...restprops } = props;
  const formItems: FormItemFactoryProps[] = [
    {
      type: FormItemFactoryType.input,
      label: '类别名称',
      name: 'name',
      rules: [
        {
          type: 'string',
          required: true,
          max: 30,
          message: '分类名称必填（且长度不超过30的字符）',
        },
        // { pattern: /^[\u4e00-\u9fa5_a-zA-Z0-9]+$/, message: '分类名称仅限于中英文数字' },
      ],
    },
    {
      type: FormItemFactoryType.input_number,
      label: '排序数字',
      name: 'order',
      tooltip: '列表展示时根据这个值从大到小排列',
      rules: [{ type: 'number', min: 0, message: '请输入正确数字' }],
    },
  ];

  return (
    <EditDrawerForm<ProductApi.CategoryMo>
      edit_fetch={(formVals, _) => {
        const reqBody = {
          parent_id: record.id,
          name: formVals.name,
          order: formVals.order,
        };
        return createCategory(reqBody);
      }}
      visible={record.id != '-1'}
      callback={(res) => callback(res)}
      form_items={formItems}
      title="添加分类信息"
      {...restprops}
    />
  );
}
