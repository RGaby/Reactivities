import { Fragment } from 'react'
import 'semantic-ui-css/semantic.min.css'
import { Container } from 'semantic-ui-react';
import NavBar from './NavBar';
import { observer } from 'mobx-react-lite';
import { Outlet, useLocation } from 'react-router-dom';
import HomePage from '../../features/home/HomePage';

function App() {
  const localtion = useLocation();

  return (
    <Fragment>
      {localtion.pathname === '/' ? <HomePage /> : (
        <>
          <NavBar />
          <Container style={{ marginTop: '6em' }}>
            <Outlet />
          </Container>
        </>
      )}

    </Fragment>

  )
}

export default observer(App);

