
// import React, { useState, useEffect } from 'react';
// import Header from './Header';
// import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
// import { faEdit } from '@fortawesome/free-solid-svg-icons/faEdit';
// import { faBars, faEnvelope, faFolderTree, faIdCard, faLocationDot, faMobile, faUser } from '@fortawesome/free-solid-svg-icons';
// import { faBuilding } from '@fortawesome/free-solid-svg-icons/faBuilding';
// import { jwtDecode } from 'jwt-decode';
// import Cookies from 'js-cookie';
// import axios from 'axios';



// const Profile = () => {
//   const [isEditable, setIsEditable] = useState(false);
//   const [userEmail, setUserEmail] = useState('');
//   const [userPhone, setUserPhone] = useState('');
//   const [userId, setUserId] = useState('');

//   const [profileData, setProfileData] = useState(() => {
//     // Load initial state from local storage if available
//     const savedProfileData = localStorage.getItem('profileData');
//     return savedProfileData ? JSON.parse(savedProfileData) : {
//       userId: '',
//       name: '',
//       userMail: '',
//       gender: '',
//       phoneNumber: '',
//       address: '',
//       dept: '',
//       designation: '',
//       branch: '',
//     };
//   });

//   const handleInputChange = (e) => {
//     const { name, value } = e.target;
//     setProfileData((prevData) => {
//       const newData = { ...prevData, [name]: value || '' }; // Default to empty string if value is falsy
//       localStorage.setItem('profileData', JSON.stringify(newData)); // Update local storage
//       return newData;
//     });
//   };

//   const [profileImage, setProfileImage] = useState('/Images/profile-img.jpg'); // Default profile image
//   const [imagePreview, setImagePreview] = useState(profileImage);

//   useEffect(() => {
//     const token = Cookies.get('token');

//     if (token) {
//       try {
//         const decoded = jwtDecode(token); 
      
//         setProfileData((prevData) => ({
//           ...prevData,
//           name: decoded.unique_name || prevData.name,
           
//         }));
//       } catch (error) {
//         console.error("Failed to decode token:", error);
//       }

//     }
    
//   }, []);

  
//   useEffect(() => {
//     const token = Cookies.get('token');
//     if (token) {
//       const decode = jwtDecode(token);
//       console.log('Decoded token payload:', decode);
      
//       // Extract the userId from the 'nameid' field
//       const userId = decode.nameid;

//       const fetchUserDetails = async () => {
//         try {
//           // Make an API request to fetch user details
//           const userResponse = await axios.get(`https://localhost:7287/api/Users/${userId}`, {
//             headers: {
//               Authorization: `Bearer ${token}` // Passing the token for authorization
//             }
//           });

//           const userDetails = userResponse.data;
//           console.log(userDetails);
//           // Update email and phone number in state
//           setUserEmail(userDetails.userMail);
//           setUserPhone(userDetails.phoneNumber);
//           setProfileData(userDetails);

//           // Optionally store them in cookies for future use
//           Cookies.set('userEmail', userDetails.userMail);
//           Cookies.set('userPhone', userDetails.phoneNumber);
//         } catch (error) {
//           console.error('Error fetching user details:', error);
//         }
//       };

//       // Make the API call after extracting the userId
//       fetchUserDetails();
//     }
//   }, []);



//   const toggleEditMode = async () => {
//     if (isEditable) {
//       const token = Cookies.get('token');
//       if (token) {
//         const decode = jwtDecode(token);
//         const userIdFromToken = decode.nameid; 
//         const userRole=decode.role;
  
//         // Prepare updated profile data, filtering out empty or undefined fields
//         const updatedProfileData = {
//           userId: userIdFromToken,
//           userName: profileData.name || '',  // Set default empty string if not available
//           gender: profileData.gender || '',
//           dept: profileData.dept || '',
//           address: profileData.address || '',
//           designation: profileData.designation || '',
//           branch: profileData.branch || '',
//           phoneNumber: userPhone || '',  // Handle undefined phoneNumber
//           userMail: userEmail || '', 
//           user_Type: userRole  
//         };
  
//         console.log('Data being sent:', updatedProfileData);
//         try {
//           const response = await axios.put(
//             `https://localhost:7287/api/Users/${userIdFromToken}`, 
//             updatedProfileData, 
//             {
//               headers: {
//                 Authorization: `Bearer ${token}`,
//                 'Content-Type': 'application/json',
//               },
//             }
//           );
//           console.log('Profile updated:', response.data);
//           setProfileData(updatedProfileData);
//           localStorage.setItem('profileData', JSON.stringify(updatedProfileData));
           
//         } catch (error) {
//           if (error.response && error.response.data.errors) {
//             console.error('Validation errors:', error.response.data.errors);
//           } else {
//             console.error('Error updating profile:', error);
//           }
//         }
//       }
//     }
//     setIsEditable(!isEditable);
//   };



//   const handleImageUpload = (e) => {
//     const file = e.target.files[0];
//     if (file) {
//       const reader = new FileReader();
//       reader.onloadend = () => {
//         setImagePreview(reader.result); // Show image preview
//       };
//       reader.readAsDataURL(file);
//       setProfileImage(file); // Store the uploaded file
//     }
//   };

//   return (
//     <div className="min-h-screen bg-white">
//       <Header />
//       <div className="flex flex-col items-center justify-center min-h-screen bg-gray-100">
//         <div className="p-5 font-bold">
//           <h1 className="text-3xl text-indigo-950 font-bold">PROFILE</h1></div>
//         <div className="w-full max-w-4xl bg-white rounded-lg shadow-md p-6">

//           <div className="flex justify-between items-center mb-6">

//             <button
//               onClick={toggleEditMode}
//               className="bg-red-500 hover:bg-red-600 text-white py-2 px-4 rounded-md"
//             >
//               {isEditable ? 'Save Profile' : 'Update Profile'}
//             </button>
//           </div>

//           <div className="flex items-center justify-center relative mb-8">
//             {/* Profile Image */}
//             <img
//               src={imagePreview}
//               alt="Profile"
//               className="w-32 h-32 rounded-full border-2 border-gray-300 object-cover"
//             />
//             {/* Edit Icon */}

//             {isEditable && (
//               <label className="absolute bottom-0 right-0 bg-white p-1 rounded-full cursor-pointer">
//                 <input
//                   type="file"
//                   className="hidden"
//                   accept="image/*"
//                   onChange={handleImageUpload}
//                 />
//                 <FontAwesomeIcon icon={faEdit} className="h-6 w-6 text-indigo-950" />
//               </label>
//             )}

//           </div>

//           <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
//             {/* Name */}
//             <div>

//               <label className="block text-sm font-medium text-gray-700">Name</label>
//               <div className="mt-1 relative flex items-center">
//                 <FontAwesomeIcon
//                   icon={faIdCard}
//                   className={`absolute left-3 h-3 w-3 ${isEditable ? 'text-slate-300' : 'text-indigo-950'
//                     }`}
//                 />

//                 <input

//                   type="text"
//                   name="name"
//                   value={profileData.name}
//                   onChange={handleInputChange}
//                   disabled={!isEditable}
//                   className={`mt-1 block w-full px-8 py-2 border ${isEditable ? 'border-gray-300' : 'border-transparent'
//                     } rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm ${isEditable ? 'bg-indigo-950 text-slate-300' : 'bg-gray-100 text-indigo-950 font-semibold'
//                     }`}
//                 />
//               </div>
//             </div>

//             {/* Email */}
//             <div>
//               <label className="block text-sm font-medium text-gray-700">Mail Address</label>
//               <div className="mt-1 relative flex items-center">
//                 <FontAwesomeIcon
//                   icon={faEnvelope}
//                   className={`absolute left-3 h-3 w-3 ${isEditable ? 'text-slate-300' : 'text-indigo-950'
//                     }`}
//                 />
//                 <input
//                   type="email"
//                   name="email"
//                   value={userEmail}
//                   onChange={handleInputChange}
//                   disabled={!isEditable}
//                   className={`mt-1 block w-full px-8 py-2 border ${isEditable ? 'border-gray-300' : 'border-transparent'
//                     } rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm ${isEditable ? 'bg-indigo-950 text-slate-300' : 'bg-gray-100 text-indigo-950 font-semibold'
//                     }`}
//                 />
//               </div>
//             </div>

//             {/* Gender */}
//             <div>
//               <label className="block text-sm font-medium text-gray-700">Gender</label>
//               <div className="mt-1 relative flex items-center">
//                 <FontAwesomeIcon
//                   icon={faUser}
//                   className={`absolute left-3 h-3 w-3 ${isEditable ? 'text-slate-300' : 'text-indigo-950'
//                     }`}
//                 />
//                 <input
//                   type="text"
//                   name="gender"
//                   value={profileData.gender}
//                   onChange={handleInputChange}
//                   disabled={!isEditable}
//                   className={`mt-1 block w-full px-8 py-2 border ${isEditable ? 'border-gray-300' : 'border-transparent'
//                     } rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm ${isEditable ? 'bg-indigo-950 text-slate-300' : 'bg-gray-100 text-indigo-950 font-semibold'
//                     }`}
//                 />
//               </div>
//             </div>

//             {/* Department */}
//             {/* Department */}
//             <div>
//               <label className="block text-sm font-medium text-gray-700">Department</label>
//               <div className="mt-1 relative flex items-center">
//                 <FontAwesomeIcon
//                   icon={faFolderTree}
//                   className={`absolute left-3 h-3 w-3 ${isEditable ? 'text-slate-300' : 'text-indigo-950'}`}
//                 />
//                 <input
//                   type="text"
//                   name="dept" // Change from "department" to "dept"
//                   value={profileData.dept} // Ensure the state key matches
//                   onChange={handleInputChange}
//                   disabled={!isEditable}
//                   className={`mt-1 block w-full px-8 py-2 border ${isEditable ? 'border-gray-300' : 'border-transparent'} rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm ${isEditable ? 'bg-indigo-950 text-slate-300' : 'bg-gray-100 text-indigo-950 font-semibold'}`}
//                 />
//               </div>
//             </div>


//             {/* Contact Number */}
//             <div>
//               <label className="block text-sm font-medium text-gray-700">Contact Number</label>
//               <div className="mt-1 relative flex items-center">
//                 <FontAwesomeIcon
//                   icon={faMobile}
//                   className={`absolute left-3 h-3 w-3 ${isEditable ? 'text-slate-300' : 'text-indigo-950'
//                     }`}
//                 />

//                 <input
//                   type="text"
//                   name="contact"
//                   value={userPhone}
//                   onChange={handleInputChange}
//                   disabled={!isEditable}
//                   className={`mt-1 block w-full px-8 py-2 border ${isEditable ? 'border-gray-300' : 'border-transparent'
//                     } rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm ${isEditable ? 'bg-indigo-950 text-slate-300' : 'bg-gray-100 text-indigo-950 font-semibold'
//                     }`}
//                 />
//               </div>
//             </div>

//             {/* Address */}
//             <div>
//               <label className="block text-sm font-medium text-gray-700">Address</label>
//               <div className="mt-1 relative flex items-center">
//                 <FontAwesomeIcon
//                   icon={faLocationDot}
//                   className={`absolute left-3 h-3 w-3 ${isEditable ? 'text-slate-300' : 'text-indigo-950'
//                     }`}
//                 />
//                 <input
//                   type="text"
//                   name="address"
//                   value={profileData.address}
//                   onChange={handleInputChange}
//                   disabled={!isEditable}
//                   className={`mt-1 block w-full px-8 py-2 border ${isEditable ? 'border-gray-300' : 'border-transparent'
//                     } rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm ${isEditable ? 'bg-indigo-950 text-slate-300' : 'bg-gray-100 text-indigo-950 font-semibold'
//                     }`}
//                 />
//               </div>
//             </div>

//             {/* Designation */}
//             <div>
//               <label className="block text-sm font-medium text-gray-700">Designation</label>
//               <div className="mt-1 relative flex items-center">
//                 <FontAwesomeIcon
//                   icon={faBars}
//                   className={`absolute left-3 h-3 w-3 ${isEditable ? 'text-slate-300' : 'text-indigo-950'
//                     }`}
//                 />
//                 <input
//                   type="text"
//                   name="designation"
//                   value={profileData.designation}
//                   onChange={handleInputChange}
//                   disabled={!isEditable}
//                   className={`mt-1 block w-full px-8 py-2 border ${isEditable ? 'border-gray-300' : 'border-transparent'
//                     } rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm ${isEditable ? 'bg-indigo-950 text-slate-300' : 'bg-gray-100 text-indigo-950 font-semibold'
//                     }`}
//                 />
//               </div>
//             </div>

//             {/* Branch */}
//             <div>
//               <label className="block text-sm font-medium text-gray-700">Branch</label>
//               <div className="mt-1 relative flex items-center">
//                 <FontAwesomeIcon
//                   icon={faBuilding}
//                   className={`absolute left-3 h-3 w-3 ${isEditable ? 'text-slate-300' : 'text-indigo-950'
//                     }`}
//                 />
//                 <input
//                   type="text"
//                   name="branch"
//                   value={profileData.branch}
//                   onChange={handleInputChange}
//                   disabled={!isEditable}
//                   className={`mt-1 block w-full px-8 py-2 border ${isEditable ? 'border-gray-300' : 'border-transparent'
//                     } rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm ${isEditable ? 'bg-indigo-950 text-slate-300' : 'bg-gray-100 text-indigo-950 font-semibold'
//                     }`}
//                 />
//               </div>
//             </div>
//           </div>
//         </div>
//       </div>
//     </div>
//   );
// };

// export default Profile;





import React, { useState, useEffect } from 'react';
import Header from './Header';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faEdit } from '@fortawesome/free-solid-svg-icons';
import { faBars, faEnvelope, faFolderTree, faIdCard, faLocationDot, faMobile, faUser } from '@fortawesome/free-solid-svg-icons';
import { faBuilding } from '@fortawesome/free-solid-svg-icons/faBuilding';
import { jwtDecode } from 'jwt-decode';
import Cookies from 'js-cookie';
import axios from 'axios';

const Profile = () => {
  const [isEditable, setIsEditable] = useState(false);
  const [userEmail, setUserEmail] = useState('');
  const [userPhone, setUserPhone] = useState('');
  const [userId, setUserId] = useState('');

  const [profileData, setProfileData] = useState(() => {
    // Load initial state from local storage if available
    const savedProfileData = localStorage.getItem('profileData');
    return savedProfileData ? JSON.parse(savedProfileData) : {
      userId: '',
      name: '',
      userMail: '',
      gender: '', // Default to empty string for new users
      phoneNumber: '',
      address: '',
      dept: '', // Default to empty string for new users
      designation: '', // Default to empty string for new users
      branch: '',
    };
  });

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setProfileData((prevData) => {
      const newData = { ...prevData, [name]: value || '' }; // Default to empty string if value is falsy
      localStorage.setItem('profileData', JSON.stringify(newData)); // Update local storage
      return newData;
    });
  };

  const [profileImage, setProfileImage] = useState('/Images/profile-img.jpg'); // Default profile image
  const [imagePreview, setImagePreview] = useState(profileImage);

  useEffect(() => {
    const token = Cookies.get('token');

    if (token) {
      try {
        const decoded = jwtDecode(token); 
      
        setProfileData((prevData) => ({
          ...prevData,
          name: decoded.unique_name || prevData.name,
        }));
      } catch (error) {
        console.error("Failed to decode token:", error);
      }
    }
  }, []);

  useEffect(() => {
    const token = Cookies.get('token');
    if (token) {
      const decode = jwtDecode(token);
      console.log('Decoded token payload:', decode);
      
      // Extract the userId from the 'nameid' field
      const userId = decode.nameid;

      const fetchUserDetails = async () => {
        try {
          // Make an API request to fetch user details
          const userResponse = await axios.get(`https://localhost:7287/api/Users/${userId}`, {
            headers: {
              Authorization: `Bearer ${token}` // Passing the token for authorization
            }
          });

          const userDetails = userResponse.data;
          console.log(userDetails);

          // Update email and phone number in state
          setUserEmail(userDetails.userMail);
          setUserPhone(userDetails.phoneNumber);

          // Update profile data, setting fields to empty strings if null
          setProfileData(prevData => ({
            ...prevData,
            ...userDetails,
            gender: userDetails.gender || '', // Ensure it's empty if null
            dept: userDetails.dept || '',     // Ensure it's empty if null
            designation: userDetails.designation || '', // Ensure it's empty if null
          }));

          // Optionally store them in cookies for future use
          Cookies.set('userEmail', userDetails.userMail);
          Cookies.set('userPhone', userDetails.phoneNumber);
        } catch (error) {
          console.error('Error fetching user details:', error);
        }
      };

      // Make the API call after extracting the userId
      fetchUserDetails();
    }
  }, []);

  const toggleEditMode = async () => {
    if (isEditable) {
      const token = Cookies.get('token');
      if (token) {
        const decode = jwtDecode(token);
        const userIdFromToken = decode.nameid; 
        const userRole = decode.role;

        // Prepare updated profile data, filtering out empty or undefined fields
        const updatedProfileData = {
          userId: userIdFromToken,
          userName: profileData.name || '',  // Set default empty string if not available
          gender: profileData.gender || '',
          dept: profileData.dept || '',
          address: profileData.address || '',
          designation: profileData.designation || '',
          branch: profileData.branch || '',
          phoneNumber: userPhone || '',  // Handle undefined phoneNumber
          userMail: userEmail || '', 
          user_Type: userRole  
        };

        console.log('Data being sent:', updatedProfileData);
        try {
          const response = await axios.put(
            `https://localhost:7287/api/Users/${userIdFromToken}`, 
            updatedProfileData, 
            {
              headers: {
                Authorization: `Bearer ${token}`,
                'Content-Type': 'application/json',
              },
            }
          );
          console.log('Profile updated:', response.data);
          setProfileData(updatedProfileData);
          localStorage.setItem('profileData', JSON.stringify(updatedProfileData));
        } catch (error) {
          if (error.response && error.response.data.errors) {
            console.error('Validation errors:', error.response.data.errors);
          } else {
            console.error('Error updating profile:', error);
          }
        }
      }
    }
    setIsEditable(!isEditable);
  };

  const handleImageUpload = (e) => {
    const file = e.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onloadend = () => {
        setImagePreview(reader.result); // Show image preview
      };
      reader.readAsDataURL(file);
      setProfileImage(file); // Store the uploaded file
    }
  };

  return (
    <div className="min-h-screen bg-white">
      <Header />
      <div className="flex flex-col items-center justify-center min-h-screen bg-gray-100">
        <div className="p-5 font-bold">
          <h1 className="text-3xl text-indigo-950 font-bold">PROFILE</h1>
        </div>
        <div className="w-full max-w-4xl bg-white rounded-lg shadow-md p-6">
          <div className="flex justify-between items-center mb-6">
            <button
              onClick={toggleEditMode}
              className="bg-red-500 hover:bg-red-600 text-white py-2 px-4 rounded-md"
            >
              {isEditable ? 'Save Profile' : 'Update Profile'}
            </button>
          </div>

          <div className="flex items-center justify-center relative mb-8">
            {/* Profile Image */}
            <img
              src={imagePreview}
              alt="Profile"
              className="w-32 h-32 rounded-full border-2 border-gray-300 object-cover"
            />
            {/* Edit Icon */}
            {isEditable && (
              <label className="absolute bottom-0 right-0 bg-white p-1 rounded-full cursor-pointer">
                <input
                  type="file"
                  className="hidden"
                  accept="image/*"
                  onChange={handleImageUpload}
                />
                <FontAwesomeIcon icon={faEdit} className="h-6 w-6 text-indigo-950" />
              </label>
            )}
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            {/* Name */}
            <div>
              <label className="block text-sm font-medium text-gray-700">Name</label>
              <div className="mt-1 relative flex items-center">
                <FontAwesomeIcon
                  icon={faIdCard}
                  className={`absolute left-3 h-3 w-3 ${isEditable ? 'text-slate-300' : 'text-indigo-950'}`}
                />
                <input
                  type="text"
                  name="name"
                  value={profileData.name}
                  onChange={handleInputChange}
                  disabled={!isEditable}
                  className={`mt-1 block w-full px-8 py-2 border ${isEditable ? 'border-gray-300' : 'border-transparent'} rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm ${isEditable ? 'bg-indigo-950 text-slate-300' : 'bg-gray-100 text-indigo-950 font-semibold'}`}
                />
              </div>
            </div>

            {/* Email */}
            <div>
              <label className="block text-sm font-medium text-gray-700">Mail Address</label>
              <div className="mt-1 relative flex items-center">
                <FontAwesomeIcon
                  icon={faEnvelope}
                  className={`absolute left-3 h-3 w-3 ${isEditable ? 'text-slate-300' : 'text-indigo-950'}`}
                />
                <input
                  type="email"
                  name="userMail"
                  value={userEmail}
                  onChange={handleInputChange}
                  disabled={!isEditable}
                  className={`mt-1 block w-full px-8 py-2 border ${isEditable ? 'border-gray-300' : 'border-transparent'} rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm ${isEditable ? 'bg-indigo-950 text-slate-300' : 'bg-gray-100 text-indigo-950 font-semibold'}`}
                />
              </div>
            </div>

            {/* Phone Number */}
            <div>
              <label className="block text-sm font-medium text-gray-700">Phone Number</label>
              <div className="mt-1 relative flex items-center">
                <FontAwesomeIcon
                  icon={faMobile}
                  className={`absolute left-3 h-3 w-3 ${isEditable ? 'text-slate-300' : 'text-indigo-950'}`}
                />
                <input
                  type="text"
                  name="phoneNumber"
                  value={userPhone}
                  onChange={handleInputChange}
                  disabled={!isEditable}
                  className={`mt-1 block w-full px-8 py-2 border ${isEditable ? 'border-gray-300' : 'border-transparent'} rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm ${isEditable ? 'bg-indigo-950 text-slate-300' : 'bg-gray-100 text-indigo-950 font-semibold'}`}
                />
              </div>
            </div>

            {/* Gender */}
            <div>
              <label className="block text-sm font-medium text-gray-700">Gender</label>
              <div className="mt-1 relative flex items-center">
                <FontAwesomeIcon
                  icon={faUser}
                  className={`absolute left-3 h-3 w-3 ${isEditable ? 'text-slate-300' : 'text-indigo-950'}`}
                />
                <input
                  type="text"
                  name="gender"
                  value={profileData.gender}
                  onChange={handleInputChange}
                  disabled={!isEditable}
                  className={`mt-1 block w-full px-8 py-2 border ${isEditable ? 'border-gray-300' : 'border-transparent'} rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm ${isEditable ? 'bg-indigo-950 text-slate-300' : 'bg-gray-100 text-indigo-950 font-semibold'}`}
                />
              </div>
            </div>

            {/* Department */}
            <div>
              <label className="block text-sm font-medium text-gray-700">Department</label>
              <div className="mt-1 relative flex items-center">
                <FontAwesomeIcon
                  icon={faBuilding}
                  className={`absolute left-3 h-3 w-3 ${isEditable ? 'text-slate-300' : 'text-indigo-950'}`}
                />
                <input
                  type="text"
                  name="dept"
                  value={profileData.dept}
                  onChange={handleInputChange}
                  disabled={!isEditable}
                  className={`mt-1 block w-full px-8 py-2 border ${isEditable ? 'border-gray-300' : 'border-transparent'} rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm ${isEditable ? 'bg-indigo-950 text-slate-300' : 'bg-gray-100 text-indigo-950 font-semibold'}`}
                />
              </div>
            </div>

            {/* Designation */}
            <div>
              <label className="block text-sm font-medium text-gray-700">Designation</label>
              <div className="mt-1 relative flex items-center">
                <FontAwesomeIcon
                  icon={faFolderTree}
                  className={`absolute left-3 h-3 w-3 ${isEditable ? 'text-slate-300' : 'text-indigo-950'}`}
                />
                <input
                  type="text"
                  name="designation"
                  value={profileData.designation}
                  onChange={handleInputChange}
                  disabled={!isEditable}
                  className={`mt-1 block w-full px-8 py-2 border ${isEditable ? 'border-gray-300' : 'border-transparent'} rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm ${isEditable ? 'bg-indigo-950 text-slate-300' : 'bg-gray-100 text-indigo-950 font-semibold'}`}
                />
              </div>
            </div>

            {/* Address */}
            <div className="col-span-2">
              <label className="block text-sm font-medium text-gray-700">Address</label>
              <div className="mt-1 relative flex items-center">
                <FontAwesomeIcon
                  icon={faLocationDot}
                  className={`absolute left-3 h-3 w-3 ${isEditable ? 'text-slate-300' : 'text-indigo-950'}`}
                />
                <input
                  type="text"
                  name="address"
                  value={profileData.address}
                  onChange={handleInputChange}
                  disabled={!isEditable}
                  className={`mt-1 block w-full px-8 py-2 border ${isEditable ? 'border-gray-300' : 'border-transparent'} rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm ${isEditable ? 'bg-indigo-950 text-slate-300' : 'bg-gray-100 text-indigo-950 font-semibold'}`}
                />
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Profile;
