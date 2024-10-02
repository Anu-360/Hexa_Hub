// /* eslint-disable no-unused-vars */
// import { jwtDecode } from "jwt-decode";
// import Cookies from 'js-cookie';

// export const jwtToken = () => {
//     const token = Cookies.get('token');
//     const decode = jwtDecode(token);
//     const decodedjwt = Cookies.get(decode);
// }

import { jwtDecode } from "jwt-decode";
import Cookies from 'js-cookie';

export const jwtToken = () => {
    const token = Cookies.get('token');
    if (!token) {
        return null; // Return null if no token is found
    }
    try {
        return jwtDecode(token); // Decode the token
    } catch (error) {
        console.error('Error decoding token:', error);
        return null;
    }
};