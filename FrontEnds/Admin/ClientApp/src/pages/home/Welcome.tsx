import React from 'react';
import { PageHeaderWrapper } from '@ant-design/pro-layout';
import { Card, Typography } from 'antd';

const {  Paragraph } = Typography;

export default (props: any): React.ReactNode => {
  return (
    <PageHeaderWrapper {...props} title="欢迎使用！">
      <Card>
        <Typography>        
          <Paragraph>
          欢迎使用！
          </Paragraph>
        </Typography>
      </Card>
    </PageHeaderWrapper>
  );
};
