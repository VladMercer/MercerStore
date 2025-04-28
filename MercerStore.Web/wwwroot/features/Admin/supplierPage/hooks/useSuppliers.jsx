import {useSelector} from 'react-redux';

export const useSuppliers = () => {


    const suppliers = useSelector((state) => state.supplierPage.suppliers);
    const pageNumber = useSelector((state) => state.supplierPage.pageNumber);
    const pageSize = useSelector((state) => state.supplierPage.pageSize);
    const totalPages = useSelector((state) => state.supplierPage.totalPages);
    const totalSuppliers = useSelector((state) => state.supplierPage.totalSuppliers);
    const isLoaded = useSelector((state) => state.supplierPage.isLoaded);
    const isPageReset = useSelector((state) => state.supplierPage.isPageReset);
    const query = useSelector((state) => state.supplierPage.query);

    return {
        query,
        suppliers,
        pageNumber,
        pageSize,
        totalPages,
        totalSuppliers,
        isLoaded,
        isPageReset,
    };
};