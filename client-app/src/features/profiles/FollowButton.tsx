import { SyntheticEvent } from "react";
import { Profile } from "../../app/model/profile";
import { Button, Reveal } from "semantic-ui-react";
import { observer } from "mobx-react-lite";
import { useStore } from "../../app/stores/store";

interface Props {
    profile: Profile;
}

export default observer(function FollowButton({ profile }: Props) {

    const { profileStore, userStore } = useStore();
    const { updateFollowing, loading } = profileStore;

    if (userStore.user?.username === profile.userName) return null;

    function handleFollow(e: SyntheticEvent, username: string) {
        e.preventDefault();
        updateFollowing(username, !profile.following);
    }

    return (
        <Reveal animated="move">
            <Reveal.Content visible style={{ width: '100%' }}>
                <Button fluid color="teal" content={profile.following ? 'Following' : 'Not Following'} />
            </Reveal.Content>
            <Reveal.Content hidden style={{ width: '100%' }}>
                <Button
                    fluid
                    basic
                    color={profile.following ? 'red' : 'green'}
                    content={profile.following ? 'Unfollow' : 'Follow'}
                    loading={loading}
                    disabled={loading}
                    onClick={(e) => handleFollow(e, profile.userName)} />
            </Reveal.Content>
        </Reveal>
    )
});