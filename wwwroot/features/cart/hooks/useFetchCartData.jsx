import { useSelector, useDispatch } from 'react-redux';
import { useEffect } from 'react';
import { fetchCartData, generateToken } from '../redux/cartSlice';

const useFetchCartData = () => {
    const dispatch = useDispatch();
    const { items, isLoaded } = useSelector((state) => state.cart);

    useEffect(() => {
        if (!isLoaded) {
            dispatch(generateToken())
            dispatch(fetchCartData());

        }
    }, [isLoaded, dispatch]);

    return items;
};
export default useFetchCartData;