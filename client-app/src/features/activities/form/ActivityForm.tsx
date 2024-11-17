import { useEffect, useState } from "react";
import { Button, Header, Segment } from "semantic-ui-react";
import { useStore } from "../../../app/stores/store";
import { observer } from "mobx-react-lite";
import { Link, useNavigate, useParams } from "react-router-dom";
import { IActivity, ActivityFormValues } from "../../../app/model/activity";
import { v4 as uuid } from 'uuid';
import LoadingComponent from "../../../app/layout/LoadingComponent";
import { Formik, Form } from "formik";
import * as Yup from 'yup';
import MyTextInput from "../../../app/api/common/form/MyTextInput";
import MyTextArea from "../../../app/api/common/form/MyTextArea";
import MySelectInput from "../../../app/api/common/form/MySelectInput";
import { categoryOptions } from "../../../app/api/common/options/categoryOptions";
import MyDateInput from "../../../app/api/common/form/MyDateInput";


export default observer(function ActivityForm() {

    const { activityStore } = useStore();
    const { createActivity, updateActivity, loading, loadActivity, loadingInitial } = activityStore;

    const { id } = useParams();
    const navigate = useNavigate();

    const [activity, setActivity] = useState<ActivityFormValues>(new ActivityFormValues());

    const validationSchema = Yup.object({
        title: Yup.string().required('The activity title is required'),
        description: Yup.string().required('The activity description is required'),
        category: Yup.string().required(),
        date: Yup.string().required('Date is required'),
        venue: Yup.string().required(),
        city: Yup.string().required()
    })

    useEffect(() => {
        if (id) loadActivity(id).then(activity => setActivity(new ActivityFormValues(activity)))
    }, [id, loadActivity])


    function handleFormSubmit(activity: ActivityFormValues) {

        console.log(`my activity ${activity}`);
        if (!activity.id) {
            let newActivity = {
                ...activity,
                id: uuid()
            }
            console.log(`my activity id ${newActivity.id}`);
            createActivity(newActivity).then(() => navigate(`/activities/${newActivity.id}`));
        }
        else {
            updateActivity(activity).then(() => navigate(`/activities/${activity.id}`));;
        }

    }

    if (loadingInitial) return <LoadingComponent content="Loading activity..." />


    return (
        <Segment clearing>
            <Header content="Activity Details" sub color='teal' />
            <Formik
                validationSchema={validationSchema}
                enableReinitialize
                initialValues={activity}
                onSubmit={values => handleFormSubmit(values)}>
                {
                    ({ handleSubmit, isValid, isSubmitting, dirty }) => (
                        <Form className="ui form" onSubmit={handleSubmit} autoComplete='off'>
                            <MyTextInput name='title' placeholder="Title"></MyTextInput>
                            <MyTextArea rows={5} placeholder='Description' name='description' />
                            <MySelectInput options={categoryOptions} placeholder='Category' name='category' />
                            <MyDateInput
                                placeholderText='Date'
                                showTimeSelect
                                timeCaption="time"
                                dateFormat="MMMM d, yyyy h:mm aa"
                                name='date'
                                onChange={_ => console.log()}
                            />
                            <Header content="Location Details" sub color='teal' />
                            <MyTextInput placeholder='City' name='city' />
                            <MyTextInput placeholder='Venue' name='venue' />
                            <Button
                                disabled={isSubmitting || !dirty || !isValid}
                                loading={loading} floated="right" positive type='submit' content='Submit' />
                            <Button as={Link} to='/activities' floated="right" type='button' content='Cancel' />
                        </Form>
                    )
                }
            </Formik>

        </Segment >
    )
})