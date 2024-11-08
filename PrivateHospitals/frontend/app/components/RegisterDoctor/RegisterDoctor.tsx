'use client'

import React, { useState } from 'react'
import { DoctorRequest } from '../../services/doctor';
import styles from './RegisterPage.module.css';
import { doctorSchema } from '@/app/validation/doctorValidation';
import { useRouter } from 'next/navigation';





const RegisterDoctor = () => {
  const [firstName, setFirstName] = useState<string>("");
  const [lastName, setLastName] = useState<string>("");
  const [userName, setUserName] = useState<string>("");
  const [email, setEmail] = useState<string>("");
  const [password, setPassword] = useState<string>("");
  const [speciality, setSpeciality] = useState<string>("");
  const route = useRouter();

  const handleCreate = async (event: React.FormEvent) => {
    event.preventDefault();
    const doctorRequest: DoctorRequest = {
      Email: email,
      FirstName: firstName,
      LastName: lastName,
      Password: password,
      Speciality: speciality,
      UserName: userName
    };

    const result = doctorSchema.safeParse(doctorRequest);
    if(!result.success){
      console.error("Validation Error", result.error.errors);
      return;
    }


    try{
      const response = await fetch("https://localhost:7262/api/doctor/RegisterDoctor", {
        method: "POST",
        headers: {
          "content-type": "application/json"
        },
        body: JSON.stringify(doctorRequest)
      });

      if(response.ok){
        console.log("Doctor was added");
        route.push("/");
      }else{
        console.error("Doctor wasn`t added");
      }
    } catch(error){
      console.error("Error:", error);
    }
  }

  return (
    <div className={styles.registerContainer}>
    <h2>Register Doctor</h2>
    <form onSubmit={handleCreate}>
      <input type="text" placeholder="First Name" required className={styles.inputField} value={firstName} onChange={(e) => setFirstName(e.target.value)} />
      <input type="text" placeholder="Last Name" required className={styles.inputField} value={lastName} onChange={(e) => setLastName(e.target.value)} />
      <input type="text" placeholder="Username" required className={styles.inputField} value={userName} onChange={(e) => setUserName(e.target.value)} />
      <input type="email" placeholder="Email" required className={styles.inputField} value={email} onChange={(e) => setEmail(e.target.value)} />
      <input type="text" placeholder="Speciality" required className={styles.inputField} value={speciality} onChange={(e) => setSpeciality(e.target.value)} />
      <input type="password" placeholder="Password" required className={styles.inputField}  value={password} onChange={(e) => setPassword(e.target.value)}/>
      <button className={styles.registerButton}>Register</button>
    </form>
  </div>
  )
}

export default RegisterDoctor