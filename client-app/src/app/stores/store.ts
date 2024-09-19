import { createContext, useContext } from "react";
import ActivityStore from "./activityStore";
import CommonStore from "./commonStore";
import UserStore from "./userStore";
import ModelStore from "./modelStore";

interface Store {
    commonStore: CommonStore;
    activityStore: ActivityStore;
    userStore: UserStore;
    modalStore: ModelStore;
}

export const store: Store = {
    commonStore: new CommonStore(),
    activityStore: new ActivityStore(),
    userStore: new UserStore(),
    modalStore: new ModelStore(),
}

export const StoreContext = createContext(store);

export function useStore() {
    return useContext(StoreContext);
}