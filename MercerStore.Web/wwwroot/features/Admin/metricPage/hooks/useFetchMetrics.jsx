import { useDispatch } from 'react-redux';
import { useEffect } from 'react';
import { fetchMetrics } from '../redux/metricPageSlice';
import { useMetrics } from './useMetrics';


const useFetchMetrics = () => {
    const dispatch = useDispatch();
    const { isLoaded } = useMetrics();

    useEffect(() => {
        if (!isLoaded) {
            dispatch(fetchMetrics());
        }
    }, [isLoaded, dispatch]);
};

export default useFetchMetrics;