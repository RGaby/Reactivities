import { Link } from "react-router-dom";
import { Button, Header, Icon, Segment } from "semantic-ui-react";

export default function NotFound() {
    return (
        <Segment placeholder>
            <Header icon>
                <Icon name='search' />
                Ooops - we've looked everywhere but could not find what are you looking for!
            </Header>
            <Segment.Inline>
                <Button as={Link} to='/activities'> Back to activities</Button>
            </Segment.Inline>
        </Segment>
    )
}