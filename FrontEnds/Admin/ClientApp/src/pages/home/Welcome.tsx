import React from 'react';
import { PageHeaderWrapper } from '@ant-design/pro-layout';
import { Card, Typography } from 'antd';

const { Title, Paragraph } = Typography;

export default (props: any): React.ReactNode => {
  return (
    <PageHeaderWrapper {...props} title="欢迎使用！">
      <Card>
        <Typography>
          <Title>Introduction</Title>
          <Paragraph>
            In the process of internal desktop applications development, many different design specs
            and implementations would be involved, which might cause designers and developers
            difficulties and duplication and reduce the efficiency of development.
          </Paragraph>
        </Typography>
      </Card>
    </PageHeaderWrapper>
  );
};
