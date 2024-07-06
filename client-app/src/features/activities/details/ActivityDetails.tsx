import { Card, Image, Icon, Button } from "semantic-ui-react";
import { Activity } from "../../../app/model/activity";
import { useStore } from "../../../app/stores/store";
import LoadingComponent from "../../../app/layout/LoadingComponent";


export default function ActivityDetails() {

    const { activityStore } = useStore();
    const { selectedActivity: activity, openForm, cancelSelectedActivity } = activityStore;

    if (!activity) return <LoadingComponent />;

    return (
        <Card>
            <Image src={`/assets/categoryImages/${activity.category}.jpg`} />
            <Card.Content>
                <Card.Header> {activity.title}</Card.Header>
                <Card.Meta> <span>{activity.date}</span></Card.Meta>
                <Card.Description> {activity.description}</Card.Description>
            </Card.Content>
            <Card.Content extra>
                <Button.Group widths='2'>
                    <Button basic onClick={() => openForm(activity.id)} color='blue' content='Edit' />
                    <Button basic onClick={cancelSelectedActivity} color='grey' content='Cancel' />
                </Button.Group>
            </Card.Content>
        </Card>
    )
}