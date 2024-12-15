import { makeAutoObservable, runInAction } from "mobx";
import { IActivity, ActivityFormValues, Activity } from "../model/activity";
import agent from "../api/agent";
import { v4 as uuid } from 'uuid'
import { format } from "date-fns";
import { store } from "./store";
import { Profile } from "../model/profile";

export default class ActivityStore {
    activityRegistry = new Map<string, IActivity>()
    selectedActivity: IActivity | undefined = undefined;
    editMode = false;
    loading = false;
    loadingInitial = false;
    activitiesLoaded = false;

    constructor() {
        makeAutoObservable(this)
    }

    get activitiesByDate() {
        return Array.from(this.activityRegistry.values()).sort((a, b) => {
            if (a.date == null && b.date == null)
                return 0;
            if (a.date == null) {
                return -1;
            }

            if (b.date == null) {
                return 1;
            }

            return a.date.getTime() - b.date.getTime()
        });
    }

    get groupedActivities() {
        return Object.entries(this.activitiesByDate.reduce((activities, activity) => {
            if (activity.date == null) {
                return activities;
            }
            const date = format(activity.date, 'dd MMM yyyy');
            activities[date] = activities[date] ? [...activities[date], activity] : [activity];

            return activities;
        }, {} as { [key: string]: IActivity[] }))
    }

    canReloadActivities = () => {
        return !this.activitiesLoaded;
    }

    loadActivities = async () => {
        try {
            this.setLoadingInitial(true);
            const activities = await agent.Activities.list();
            runInAction(() => {
                activities.forEach(activity => {
                    this.setActivity(activity);
                })
                this.setLoadingInitial(false);
                this.activitiesLoaded = true;
            });

        } catch (error) {
            console.log(error);
            this.setLoadingInitial(false);
        }
    }

    loadActivity = async (id: string) => {
        let activity = this.getActivity(id);
        if (activity) {
            this.selectedActivity = activity;
            return activity;
        }
        else {
            this.setLoadingInitial(true);
            try {
                activity = await agent.Activities.details(id);
                this.setActivity(activity);
                runInAction(() => this.selectedActivity = activity);
                this.setLoadingInitial(false);
                return activity;
            } catch (error) {
                console.log(error);
                this.setLoadingInitial(false);
            }
        }
    }
    private setActivity = (activity: IActivity) => {
        const user = store.userStore.user;
        if (user) {
            activity.isGoing = activity.attendees!.some(
                a => a.userName === user.username
            )

            activity.isHost = activity.hostUsername === user.username;
            activity.host = activity.attendees?.find(x => x.userName === activity.hostUsername);
        }
        activity.date = activity.date == null ? new Date() : new Date(activity.date);
        this.activityRegistry.set(activity.id, activity);
    }

    private getActivity = (id: string) => {
        return this.activityRegistry.get(id);
    }

    setLoadingInitial = (state: boolean) => {
        this.loadingInitial = state;
    }

    selectActivity = (id: string) => {
        this.selectedActivity = this.activityRegistry.get(id);
    }

    cancelSelectedActivity = () => {
        this.selectedActivity = undefined;
    }

    createActivity = async (activity: ActivityFormValues) => {
        console.log(`Create new activity: id = ${activity.id}`);

        const user = store.userStore.user;
        const attendee = new Profile(user!);

        try {
            await agent.Activities.create(activity);
            const newActivity = new Activity(activity);
            newActivity.hostUsername = user!.username;
            newActivity.attendees = [attendee];
            this.setActivity(newActivity);

            runInAction(() => {
                this.selectedActivity = new Activity(activity);
            })
        } catch (error) {
            console.log(error);
        }
    }

    updateActivity = async (activity: ActivityFormValues) => {
        this.loading = true;
        try {
            await agent.Activities.update(activity);
            runInAction(() => {
                if (activity.id) {
                    const updatedActivity = { ...this.getActivity(activity.id), ...activity }
                    this.activityRegistry.set(activity.id, updatedActivity as Activity);
                    this.selectedActivity = updatedActivity as Activity;
                }

            })
        } catch (error) {
            console.log(error);
            runInAction(() => {
            })
        }
    }

    deleteActivity = async (id: string) => {
        this.loading = false;
        try {
            await agent.Activities.delete(id);
            runInAction(() => {
                this.activityRegistry.delete(id);
                this.loading = false;
            })

        } catch (error) {
            console.log(error);
            runInAction(() => {
                this.loading = false;
            })
        }
    }

    updateAttendance = async () => {
        const user = store.userStore.user;
        this.loading = true;
        try {
            await agent.Activities.attend(this.selectedActivity!.id);
            runInAction(() => {
                if (this.selectedActivity?.isGoing) {
                    this.selectedActivity.attendees = this.selectedActivity.attendees?.filter(a => a.userName !== user?.username);
                    this.selectedActivity.isGoing = false;
                }
                else {
                    const attendee = new Profile(user!);
                    this.selectedActivity?.attendees?.push(attendee);
                    this.selectedActivity!.isGoing = true;
                }

                this.activityRegistry.set(this.selectedActivity!.id, this.selectedActivity!);
            })
        } catch (error) {
            console.log(error);
        } finally {
            runInAction(() => this.loading = false);
        }
    }

    cancelActivityToggle = async () => {
        this.loading = true;
        try {
            await agent.Activities.attend(this.selectedActivity!.id);
            runInAction(() => {
                this.selectedActivity!.iscancelled = !this.selectedActivity?.iscancelled;
                this.activityRegistry.set(this.selectedActivity!.id, this.selectedActivity!);
            })
        } catch (error) {
            console.log(error);
        } finally {
            runInAction(() => this.loading = false);
        }
    }

    clearSelectedActivity = () => {
        this.selectedActivity = undefined;
    }
}