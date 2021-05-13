import { lockUser } from '../service';
import React from 'react';
import { LockOutlined, UnlockOutlined } from '@ant-design/icons';
import { FuncCodes } from '@/utils/resp_d';
import { UserInfo } from '../data_d';
import { TableFetchButton } from '@/components/button/table_Fetch_buttons';

// 锁定按钮
const UserLockButton: React.FC<{
  record: UserInfo;
  isLock: boolean;
  title: string;
  listReload: () => void;
}> = ({ record, listReload, title, isLock }) => {
  return (
    <TableFetchButton<UserInfo>
      func_code={isLock ? FuncCodes.Portal_UserLock : FuncCodes.Portal_UserUnLock}
      record={record}
      fetch={(r) => lockUser(r, isLock)}
      fetchKey={(r) => r.id}
      callback={() => { listReload() }}
      icon={isLock ? <LockOutlined /> : <UnlockOutlined />}
    ></TableFetchButton>
  );
};

const UserListButtons: React.FC<{ record: UserInfo; listReload: () => void }> = ({
  record,
  listReload,
}) => {
  const s = record.status;

  return (
    <>
      {s == 0 && (
        <UserLockButton
          record={record}
          listReload={listReload}
          isLock={true}
          title="锁定当前用户"
        />
      )}
      {s == -100 && (
        <UserLockButton
          record={record}
          listReload={listReload}
          isLock={false}
          title="解锁当前用户"
        />
      )}
    </>
  );
};

export default UserListButtons;
