import { Button, Result } from 'antd';
import React from 'react';
import { history } from 'umi';

const NoFoundPage: React.FC<{}> = () => (
  <Result
    status="404"
    title="404"
    subTitle="对不起，你请求的页面当前不存在."
    extra={
      <Button type="primary" onClick={() => history.replace('/')}>
        Back Home
      </Button>
    }
  />
);

export default NoFoundPage;
