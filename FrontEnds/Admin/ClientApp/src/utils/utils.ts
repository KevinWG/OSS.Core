import { parse } from 'querystring';
import moment from 'moment';
import { ProTableReqParams, SearchReq, PageListResp } from './resp_d';

/* eslint no-useless-escape:0 import/prefer-default-export:0 */
const reg = /(((^https?:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+(?::\d+)?|(?:www.|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)$/;

export const isUrl = (path: string): boolean => reg.test(path);

export const getPageQuery = () => parse(window.location.href.split('?')[1]);

export const formatTimestamp = (time: number) => {
  return moment(time * 1000).format('YYYY-MM-DD');
};

export function CompareNotIn<S, T>(source: S[], target: T[], func: (s: S, t: T) => boolean) {
  const result: S[] = [];
  for (let index = 0; index < source.length; index++) {
    const s = source[index];

    let isHave = false;
    for (let tIndex = 0; tIndex < target.length; tIndex++) {
      const t = target[tIndex];

      if (func(s, t)) {
        isHave = true;
        break;
      }
    }
    if (!isHave) {
      result.push(s);
    }
  }
  return result;
}
