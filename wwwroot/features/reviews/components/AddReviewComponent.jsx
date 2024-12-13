import React, { useState, useRef, useEffect } from 'react';
import { useDispatch } from 'react-redux';
import { useReviews } from '../hooks/useReviews';
import { addReview } from '../redux/reviewSlice';

const AddReviewComponent = () => {
    const dispatch = useDispatch();
    const { productId, review } = useReviews();

    const [showModal, setShowModal] = useState(false);
    const [reviewText, setReviewText] = useState("");
    const [rating, setRating] = useState(1);
    const modalRef = useRef(null);
    const modalInstance = useRef(null);

    const handleShow = () => setShowModal(true);

    const handleClose = () => {
        if (modalInstance.current) modalInstance.current.hide();
        setShowModal(false);
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        const newReview = { productId, reviewText, value: rating };
        dispatch(addReview(newReview));
        handleClose();
    };

    useEffect(() => {
        if (showModal && modalRef.current) {
            modalInstance.current = new window.bootstrap.Modal(modalRef.current);
            modalInstance.current.show();
            modalRef.current.addEventListener('hidden.bs.modal', handleClose);

            return () => {
                modalRef.current.removeEventListener('hidden.bs.modal', handleClose);
                modalInstance.current = null;
            };
        }
    }, [showModal]);

    const hasReview = !review;

    return (
        <>
            {hasReview && (
                <div className="mb-3">
                    <button className="btn btn-outline-warning" onClick={handleShow}>
                        Оставить отзыв
                    </button>
                </div>
            )}

            {!hasReview && <p>Спасибо за отзыв!</p>}

            <div className="modal fade" ref={modalRef} tabIndex="-1" aria-labelledby="modalLabel" aria-hidden="true">
                <div className="modal-dialog modal-lg modal-dialog-centered" role="document">
                    <div className="modal-content">
                        <div className="modal-header">
                            <h5 className="modal-title" id="modalLabel">Добавить отзыв</h5>
                            <button type="button" className="btn-close" aria-label="Close" onClick={handleClose}></button>
                        </div>
                        <div className="modal-body">
                            <form onSubmit={handleSubmit}>
                                <div className="mb-3">
                                    <label htmlFor="reviewText" className="form-label"><h4>Опишите впечатления</h4></label>
                                    <textarea
                                        id="reviewText"
                                        className="form-control"
                                        value={reviewText}
                                        onChange={(e) => setReviewText(e.target.value)}
                                        rows="3"
                                        placeholder="Место для отзыва"
                                    />
                                </div>
                                <div className="mb-3">
                                    <div className="add-review">
                                        <fieldset className="rating">
                                            <legend className="rating__caption"><h4>Оцените продукт</h4></legend>
                                            <div className="rating__group">
                                                {[5, 4, 3, 2, 1].map((rate) => (
                                                    <React.Fragment key={rate}>
                                                        <input
                                                            type="radio"
                                                            id={`add-rate${rate}`}
                                                            value={rate}
                                                            checked={rating === rate}
                                                            onChange={() => setRating(rate)}
                                                            className="rating__star-input"
                                                        />
                                                        <label htmlFor={`add-rate${rate}`} className="rating__star">
                                                            <i className="fa-solid fa-star"></i>
                                                        </label>
                                                    </React.Fragment>
                                                ))}
                                            </div>
                                        </fieldset>
                                    </div>
                                </div>
                                <div className="modal-footer justify-content-start">
                                    <button type="submit" className="btn btn-outline-warning">
                                        Отправить отзыв
                                    </button>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </>
    );
};

export default AddReviewComponent;