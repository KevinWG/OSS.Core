import { useRequest } from 'umi';
import { message } from 'antd';
import { lockUser } from '../service';
import React from 'react';
import { LockOutlined, UnlockOutlined } from '@ant-design/icons';
import AccessButton from '@/components/Button/access_button';
import { FuncCodes } from '@/utils/resp_d';
import { UserInfo } from '../data_d';

// 锁定按钮
const UserLockButton: React.FC<{
  record: UserInfo;
  isLock: boolean;
  title: string;
  listReload: () => void;
}> = ({ record, listReload, title, isLock }) => {
  var lockReq = useRequest(lockUser, {
    manual: true,
    fetchKey: (r) => r.id,
  });

  const handler = async (item: UserInfo) => {
    message.info('开始' + title);
    var res = await lockReq.run(item, isLock);
    if (res.is_ok) {
      message.info(title + '成功！');
      listReload();
    } else {
      message.info(title + '失败:' + res.msg);
    }
  };

  return (
    <AccessButton
      func_code={isLock ? FuncCodes.Portal_UserLock : FuncCodes.Portal_UserUnLock}
      confirm_props={{
        title: '是否确认' + title + '？',
        onConfirm: () => handler(record),
        okText: '确认',
        cancelText: '放弃',
      }}
      type='dashed'
      shape='circle'
      loading={lockReq.fetches[record.id]?.loading}
      icon={isLock ? <LockOutlined /> : <UnlockOutlined />}
    ></AccessButton>
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
