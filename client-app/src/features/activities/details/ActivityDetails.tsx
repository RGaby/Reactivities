import React, { act } from "react";
import { Card, Image, Icon, Button } from "semantic-ui-react";
import { Activity } from "../../../app/model/activity";

interface Props {
    activity: Activity;
    cancelActivity: () => void;
    openForm: (id: string) => void;
}

export default function ActivityDetails({ activity, cancelActivity, openForm }: Props) {
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
                    <Button basic onClick={cancelActivity} color='grey' content='Cancel' />
                </Button.Group>
            </Card.Content>
        </Card>
    )
}