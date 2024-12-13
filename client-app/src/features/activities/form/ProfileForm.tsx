import { observer } from "mobx-react-lite";
import { Profile, ProfileInfoForm } from "../../../app/model/profile";
import { Formik } from "formik";
import * as Yup from 'yup';
import { useEffect, useState } from "react";
import { useStore } from "../../../app/stores/store";
import { Form } from "react-router-dom";
import MyTextInput from "../../../app/api/common/form/MyTextInput";
import MyTextArea from "../../../app/api/common/form/MyTextArea";
import { Button } from "semantic-ui-react";

interface Props {
    profile: Profile;
    setEditMode: (editMode: boolean) => void;
}

export default observer(function ProfileForm({ profile, setEditMode }: Props) {

    const { profileStore } = useStore();
    const { loading, updateBio } = profileStore;
    const [profileInfoForm, setProfileInfoForm] = useState<ProfileInfoForm>(new ProfileInfoForm());

    useEffect(() => {
        if (profile) {
            setProfileInfoForm(new ProfileInfoForm(profile));
        }
    }, [profile]);

    function handleFormSubmit(profileForm: ProfileInfoForm) {
        if (profileForm.displayName) {
            updateBio(profileForm).then(() => setEditMode(false));
        }
        else {
            setEditMode(false)
        }
    }

    const validationSchema = Yup.object({
        displayName: Yup.string().required('')
    });

    return (
        <Formik
            validationSchema={validationSchema}
            enableReinitialize
            initialValues={profileInfoForm}
            onSubmit={handleFormSubmit}>
            {
                ({ handleSubmit, isValid, isSubmitting, dirty }) => (
                    <Form className="ui form" onSubmit={handleSubmit} autoComplete="off">
                        <MyTextInput name='displayName' placeholder="Display Name" />
                        <MyTextArea rows={5} placeholder="Description" name='bio' />
                        <Button
                            disabled={isSubmitting || !dirty || !isValid}
                            loading={loading}
                            floated="right"
                            positive
                            type="submit"
                            content="Submit"
                        />
                    </Form>
                )
            }
        </Formik>
    );
})