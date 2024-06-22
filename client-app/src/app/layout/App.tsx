import { Fragment, useEffect, useState } from 'react'
import 'semantic-ui-css/semantic.min.css'
import axios from 'axios';
import { Container, Header, List } from 'semantic-ui-react';
import { Activity } from '../model/activity';
import NavBar from './NavBar';
import ActivityDashboard from '../../features/activities/dashboard/ActivityDashboard';

function App() {

  const [activities, setActivities] = useState<Activity[]>([]);

  useEffect(() => {
    axios.get<Activity[]>('http://localhost:5000/api/activities')
      .then(response => {
        console.log(response);
        setActivities(response.data);
      })
  }, [])

  return (
    <Fragment>
      <NavBar />
      <Container style={{ marginTop: '6em' }}>
        <ActivityDashboard activities={activities} />
      </Container>

    </Fragment>

  )
}

export default App
