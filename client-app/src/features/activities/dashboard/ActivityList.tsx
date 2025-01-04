import { Header } from "semantic-ui-react";
import { useStore } from "../../../app/stores/store";
import { observer } from "mobx-react-lite";
import { ActivityItemList } from "./ActivityListItem";
import { Fragment } from "react/jsx-runtime";


export default observer(function ActivityList() {

    const { activityStore } = useStore();
    const { groupedActivities } = activityStore;

    return (
        <>
            {groupedActivities.map(([group, activities]) => {
                return (
                    <Fragment key={group}>
                        <Header sub color='teal'>

                            {group}
                        </Header>
                        {activities.map(activity => (
                            <ActivityItemList key={activity.id} activity={activity} />
                        ))}

                    </Fragment>
                )
            })}
        </>

    )
})