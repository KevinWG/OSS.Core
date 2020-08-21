import React from 'react';
import { Form, Button, Card, Row, Col, Space, Radio, Divider } from 'antd';
import { FormProps } from 'antd/lib/form';
import FormItemFactory, { FormItemFactoryProps } from '../form/form_item_factory';
import { SizeType } from 'antd/lib/config-provider/SizeContext';

export interface SearchFormProps extends FormProps {
  items: FormItemFactoryProps[];
  reset?: () => void;

  finish_with_undifined?: boolean;
  top_radios?: { options: { label: string; value: string; disable?: boolean }[]; name: string };
  size?: SizeType;
}

const SearchForm: React.FC<SearchFormProps> = (props) => {
  const { form, items, reset, onFinish, top_radios, size, ...restProps } = props;
  const count = items.length;

  if (count == 0) {
    return <></>;
  }

  const formRef = form || Form.useForm()[0];

  const colSpan = {
    xs: 12,
    sm: 12,
    md: count == 1 ? 12 : 8,
    xl: count < 3 ? 24 / (count + 1) : 6, // 加一是因为包含查询按钮
  };

  function deleteNullFinish(vals: any) {
    if (!onFinish) return;

    for (let i in vals) {
      if (!vals[i]) {
        delete vals[i];
      }
    }
    onFinish(vals);
  }

  return (
    <div className="search-form">
      <Card size={size}>
        <Form
          form={formRef}
          onFinish={props.finish_with_undifined ? onFinish : deleteNullFinish}
          {...restProps}
        >
          {(top_radios || props.children) && (
            <Row justify="center">
              <Col span={12} key={restProps.name + '_top'}>
                {top_radios && (
                  <Form.Item name={top_radios.name} key={'item' + top_radios.name}>
                    <Radio.Group
                      optionType="button"
                      size={size}
                      name={top_radios.name}
                      options={top_radios.options}
                    ></Radio.Group>
                  </Form.Item>
                )}
              </Col>
              <Col span={12} style={{ textAlign: 'right' }}>
                {props.children}
              </Col>
              <Divider style={{ marginTop: 16, marginBottom: 12 }} />
            </Row>
          )}

          <Row style={{ marginTop: -12 }}>
            {items.map((item, index) => {
              return (
                <Col
                  {...colSpan}
                  key={restProps.name + 'col_' + index}
                  style={{ paddingRight: 15, marginTop: 18 }}
                >
                  <FormItemFactory size={size} {...item} />
                </Col>
              );
            })}

            <Col
              {...colSpan}
              key={restProps.name + 's_t_f'}
              style={{ textAlign: 'center', marginTop: 18 }}
            >
              <Form.Item>
                <Space>
                  <Button type="primary" htmlType="submit" size={size}>
                    查询
                  </Button>
                  <Button
                    size={size}
                    type="default"
                    onClick={() => {
                      formRef.resetFields();
                      reset && reset();
                    }}
                  >
                    重置
                  </Button>
                </Space>
              </Form.Item>
            </Col>
          </Row>
        </Form>
      </Card>
    </div>
  );
};

export default SearchForm;
