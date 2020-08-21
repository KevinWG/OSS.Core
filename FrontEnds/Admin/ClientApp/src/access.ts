import { AdminIdentity } from "./pages/portal/login/service";
import FuncCodes from "../config/func_codes";

// src/access.ts
export default function (initialState: { currentUser?: AdminIdentity | undefined }) {
  var defaultAccess={};
  for(var code in FuncCodes){
    defaultAccess[FuncCodes[code]]=false;
  }

  var access={};
  initialState.currentUser?.access_list?.map((v)=>{
    access[v.func_code]=true;
  });
  return Object.assign(defaultAccess,access);
}
