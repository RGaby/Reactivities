import { ErrorMessage, Form, Formik } from "formik";
import MyTextInput from "../../app/api/common/form/MyTextInput";
import { Button, Header, Label } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import { observer } from "mobx-react-lite";
import * as Yup from 'yup'

export default observer(function RegisterForm() {
    const { userStore } = useStore()
    return (
        <Formik initialValues={{ displayName: '', username: '', email: '', password: '', error: null }}
            onSubmit={(values, { setErrors }) => userStore.login(values).catch(() => setErrors({ error: 'Invalid email or password' }))}
            validationSchema={Yup.object({
                displayName: Yup.string().required(),
                username: Yup.string().required(),
                email: Yup.string().required(),
                password: Yup.string().required(),
            })}
        >

            {({ handleSubmit, isSubmitting, errors, isValid, dirty }) => (
                <Form className="ui form" onSubmit={handleSubmit} autoComplete="off">
                    <Header as='h2' content='Sign up to Reactivities' color="teal" textAlign="center"></Header>
                    <MyTextInput placeholder="Display Name" name='displayName'></MyTextInput>
                    <MyTextInput placeholder="Username" name='username'></MyTextInput>
                    <MyTextInput placeholder="Email" name='email'></MyTextInput>
                    <MyTextInput placeholder="Password" name='password' type='password'></MyTextInput>
                    <ErrorMessage
                        name='error'
                        render={
                            () => <Label style={{ marginBottom: 10 }} basic color='red' content={errors.error}></Label>
                        } />
                    <Button disabled={!isValid || !dirty || isSubmitting} loading={isSubmitting} positive content='Register' type="submit" fluid />
                </Form>
            )}
        </Formik>
    )
})