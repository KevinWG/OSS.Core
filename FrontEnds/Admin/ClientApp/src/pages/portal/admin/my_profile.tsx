import React from 'react';
import { PageHeaderWrapper } from '@ant-design/pro-layout';

import { useModel } from 'umi';
import AdminDetail from './compents/admin_detail';
import { AdminInfo } from './data_d';

const MyselfProfile: React.FC<{}> = ({}) => {
  const { initialState } = useModel('@@initialState');
  const curAdmin = initialState?.currentUser;

  const adminInfo: AdminInfo = {
    id: curAdmin?.id || '',
    admin_name: curAdmin?.name || '',
    avatar: curAdmin?.avatar || '',
  };
  return (
    <PageHeaderWrapper>
      <AdminDetail admininfo={adminInfo} is_mine={true} />
    </PageHeaderWrapper>
  );
};
export default MyselfProfile;
