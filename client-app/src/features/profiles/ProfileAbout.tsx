import { observer } from "mobx-react-lite";
import { Profile } from "../../app/model/profile";
import { useState } from "react";
import { Button, Grid, Header, TabPane } from "semantic-ui-react";
import ProfileForm from "../activities/form/ProfileForm";
import { useStore } from "../../app/stores/store";

interface Props {
    profile: Profile;
}

export default observer(function ProfileAbout({ profile }: Props) {

    const { profileStore: { isCurrentUser, loading } } = useStore();
    const [editMode, setEditMode] = useState<boolean>(false);

    return (
        <TabPane>
            <Grid>
                <Grid.Column width='16'>
                    <Header floated="left" icon='user' content={`About ${profile.displayName}`} />
                    {isCurrentUser &&
                        (<Button
                            floated="right"
                            basic
                            disabled={loading}
                            content={editMode ? 'Cancel' : 'Edit Profile'}
                            onClick={() => setEditMode(!editMode)} />)
                    }
                </Grid.Column>
                <Grid.Column width='16'>
                    {editMode ?
                        <ProfileForm profile={profile} setEditMode={setEditMode} />
                        : <span style={{ whiteSpace: 'pre-wrap' }}>{profile.bio}</span>}
                </Grid.Column>
            </Grid>
        </TabPane>
    );
})