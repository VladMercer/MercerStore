import {useDispatch, useSelector} from 'react-redux';
import {useEffect} from 'react';
import {fetchCartData, generateToken, sendHeartbeat} from '../redux/cartSlice';

const useFetchCartData = () => {
    const dispatch = useDispatch();
    const {items, isLoaded} = useSelector((state) => state.cart);

    useEffect(() => {
        if (!isLoaded) {
            dispatch(generateToken())
            dispatch(fetchCartData());

        }
    }, [isLoaded, dispatch]);

    useEffect(() => {
        const interval = setInterval(() => {
            dispatch(sendHeartbeat());
        }, 60000);

        return () => clearInterval(interval);
    }, [dispatch]);

    return items;
};
export default useFetchCartData;