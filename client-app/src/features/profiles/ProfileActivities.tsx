import { observer } from "mobx-react-lite";
import { useStore } from "../../app/stores/store";
import { SyntheticEvent, useEffect } from "react";
import { Card, Grid, Header, Tab, TabPane, TabProps, Image } from "semantic-ui-react";
import { Link } from "react-router-dom";
import { UserActivity } from "../../app/model/userActivity";
import { format } from "date-fns";

const panes = [
    { menuItem: 'Future Events', pane: { key: 'future' } },
    { menuItem: 'Past Events', pane: { key: 'past' } },
    { menuItem: 'Hosting Events', pane: { key: 'hosting' } }
]

export default observer(function ProfileActivities() {

    const { profileStore } = useStore();
    const { loadingActivities, userActivities, profile, loadUserActivities } = profileStore;

    useEffect(() => {
        loadUserActivities(profile!.userName)
    }, [loadUserActivities, profile]);

    const handleOnTabChanged = (_: SyntheticEvent, data: TabProps) => {
        loadUserActivities(profile!.userName, panes[data.activeIndex as number].pane.key);
    }

    return (
        <TabPane loading={loadingActivities}>
            <Grid>
                <Grid.Column width={16}>
                    <Header floated="left" icon='calendar' content={'Activities'} />
                </Grid.Column>
                <Grid.Column width={16}>
                    <Tab panes={panes} menu={{ secundary: true, pointing: true }}
                        onTabChange={(e, data) => handleOnTabChanged(e, data)} />
                    <br />
                    <Card.Group itemsPerRow={4}>
                        {
                            userActivities.map((activity: UserActivity) => (
                                <Card as={Link}
                                    to={`/activities/${activity.id}`}
                                    key={activity.id}>
                                    <Image src={`/assets/categoryImages/${activity.category}.jpg`}
                                        style={{ minHeight: 100, objectFit: 'cover' }}></Image>
                                    <Card.Content>
                                        <Card.Header textAlign="center">
                                            {activity.title}
                                        </Card.Header>
                                        <Card.Meta textAlign='center'>
                                            <div>{format(new Date(activity.date!), 'do LLL')} </div>
                                            <div>{format(new Date(activity.date!), 'h:mm a')}</div>
                                        </Card.Meta>
                                    </Card.Content>
                                </Card>
                            ))
                        }
                    </Card.Group>
                </Grid.Column>
            </Grid>
        </TabPane>
    )
})