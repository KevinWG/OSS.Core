import { DrawerProps } from 'antd/lib/drawer';

import EditDrawerForm from '@/components/form/eidt_drawer_form';
import { FormItemFactoryProps, FormItemFactoryType } from '@/components/form/form_item_factory';
import { updateCategorName, updateCategorOrder } from '../service';

interface AddCategoryProps extends IEditorProps<ProductApi.CategoryMo>, DrawerProps {
  edit_type: number; // 1- 修改名称， 2- 修改排序
}

export default function EditCategory(props: AddCategoryProps) {
  const { callback, edit_type, record, ...restprops } = props;
  const formItems: FormItemFactoryProps[] = [
    {
      type: FormItemFactoryType.input,
      label: '类别名称',
      name: 'name',
      hidden: edit_type != 1,
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
      hidden: edit_type != 2,
      tooltip: '列表展示时根据这个值从大到小排列',
      rules: [{ type: 'number', min: 0, message: '请输入正确数字' }],
    },
  ];

  return (
    <EditDrawerForm<ProductApi.CategoryMo>
      edit_fetch={(formVals, _) => {
        const reqBody = { name: formVals.name, order: formVals.order };
        return edit_type == 1
          ? updateCategorName(record.id, reqBody)
          : updateCategorOrder(record.id, reqBody);
      }}
      visible={record.id != '-1'}
      record={record}
      callback={(res) => callback(res)}
      form_items={formItems}
      title="修改 分类信息"
      {...restprops}
    />
  );
}
