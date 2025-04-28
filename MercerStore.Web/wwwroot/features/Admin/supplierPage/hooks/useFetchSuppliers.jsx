import {useDispatch} from 'react-redux';
import {useEffect, useRef} from 'react';
import {fetchSuppliers, setIsPageReset, setPageNumber} from '../redux/supplierPageSlice';
import {useSuppliers} from './useSuppliers';


const useFetchSuppliers = () => {
    const dispatch = useDispatch();
    const {pageNumber, pageSize, isLoaded, isPageReset, query} = useSuppliers();

    const prevQuery = useRef(query);

    const dispatchFetchSuppliers = () => {
        dispatch(fetchSuppliers({
            pageNumber, pageSize, query
        }));
    }

    useEffect(() => {


        if (!isLoaded) {
            dispatchFetchSuppliers();
        } else if (pageNumber > 1 && (query !== prevQuery.current) && !isPageReset) {
            dispatch(setPageNumber(1));
            dispatch(setIsPageReset(true));
        } else if (!isPageReset || (isPageReset && pageNumber === 1)) {

            dispatchFetchSuppliers();

            if (isPageReset && pageNumber === 1) {
                dispatch(setIsPageReset(false));
            }
        }

        prevQuery.current = query;
    }, [pageNumber, pageSize, query]);
};

export default useFetchSuppliers;