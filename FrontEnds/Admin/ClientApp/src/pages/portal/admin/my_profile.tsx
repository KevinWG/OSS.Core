import React from 'react';
import BodyContent from '@/layouts/compents/body_content';

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
    <BodyContent>
      <AdminDetail admininfo={adminInfo} is_mine={true} />
    </BodyContent>
  );
};
export default MyselfProfile;
