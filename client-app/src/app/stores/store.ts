import { createContext, useContext } from "react";
import ActivityStore from "./activityStore";
import CommonStore from "./commonStore";
import UserStore from "./userStore";
import ModelStore from "./modelStore";
import ProfileStore from "./profileStore";
import CommentStore from "./commentStore";

interface Store {
    commonStore: CommonStore;
    activityStore: ActivityStore;
    userStore: UserStore;
    modalStore: ModelStore;
    profileStore: ProfileStore;
    commentStore: CommentStore;
}

export const store: Store = {
    commonStore: new CommonStore(),
    activityStore: new ActivityStore(),
    userStore: new UserStore(),
    modalStore: new ModelStore(),
    profileStore: new ProfileStore(),
    commentStore: new CommentStore(),
}

export const StoreContext = createContext(store);

export function useStore() {
    return useContext(StoreContext);
}